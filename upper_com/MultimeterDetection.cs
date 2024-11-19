using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using S7.Net.Types;
using DateTime = System.DateTime;
using System.Windows.Forms;
using Ivi.Visa;
using NationalInstruments.Visa;
using System.IO;
using System.Diagnostics;
using System.Timers;
using System.Linq;
using Org.BouncyCastle.Bcpg.Sig;
using NPOI.POIFS.Crypt.Dsig;
using System.Collections;
using DotNetSiemensPLCToolBoxLibrary.Communication.Library;

namespace upper_com
{
    internal class MultimeterDetection
    {
        private string multimerIp;

        private MyLED myLED2;

        private DataGridView dataGridView1;

        private bool isPlaying;

        private InputData inputData;

        Queue<(int start, int duration)> timeQueue;

        private bool isCollectingData; // 控制数据采集的标志

        private int currentSerialNo; // 电流测试编号

        private CurrentDataQueue currentDataQueue;

        private VisaClient visaClient;

        public double T {  get; set; }

        public MultimeterDetection()
        {

        }

        public MultimeterDetection(DataGridView dataGridView1)
        {
            this.dataGridView1 = dataGridView1;
        }

        public MultimeterDetection(MyLED myLED2, DataGridView dataGridView1, string multimerIp)
        {
            this.multimerIp = multimerIp;
            this.myLED2 = myLED2;
            this.dataGridView1 = dataGridView1;

            visaClient = new VisaClient(this.multimerIp);
        }

        public void SetInputDate(InputData data)
        {
            this.inputData = data;
        }

        public InputData GetInputData()
        {
            return this.inputData;
        }

        public void SetIsPlaying(bool isPlaying)
        {
            this.isPlaying = isPlaying;
        }

        public bool GetIsPlaying()
        {
            return this.isPlaying;
        }

        public void SetSignal(bool signal)
        {
            this.isCollectingData = signal;
        }

        public void SetCurrentSerialNo(int serialNo)
        {
            this.currentSerialNo = serialNo;
        }


        public bool MultimerOpen()
        {
            if (visaClient.Connected)
            {
                UpdateLEDStatus(Color.Green);
                return true;
            }
            else
            {
                UpdateLEDStatus(Color.DimGray);
                return false;
            }
        }

        public void SendConfigCommand()
        {
            try
            {
                // 检查连接是否成功
                if (visaClient.Connected)
                {
                    // 发送设置命令
                    visaClient.Write("FUNCtion \"CURR:DC\"");
                    visaClient.Write("CURRent:DC:RANGe 0.2");
                    visaClient.Write("CURRent:DC:NPLC F");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送配置命令时出错: {ex.Message}");
            }
        }

        public async Task MultimeterListenerHandler()
        {
            // TODO 调试万用表测试
            if (visaClient.Connected && this.isPlaying)
            {
                await SendReadCommandAsync();
            }
            else
            {
                Console.WriteLine("数据采集停止，什么都不做！！！！！！！！");
            }
        }

        public async Task SendReadCommandAsync()
        {
            try
            {
                if (visaClient.Connected)
                {
                    await FetchgingData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending command or receiving data: {ex.Message}");
            }

        }

        private async Task FetchgingData()
        {
            if (inputData != null && inputData.DataQueue.Count > 0)
            {
                currentDataQueue = new CurrentDataQueue(this.dataGridView1, inputData.Num, inputData.K); // 初始化 DataQueue

                DateTime start;
                DateTime end;

                // 循环检查收集数据的状态，此状态由PLC设置
                while (isCollectingData && visaClient.Connected)
                {
                    // TODO 调试使用，真正需要放在收到起始信号位置开始
                    start = DateTime.Now;

                    timeQueue = new Queue<(int start, int duration)>(inputData.DataQueue);

                    // 循环遍历15组时间设置
                    while (timeQueue.Count > 0 && isCollectingData)
                    {
                        List<List<double>> stableData = new List<List<double>>();
                        List<List<double>> mutationData = new List<List<double>>();

                        CurrentData all = new CurrentData();

                        var data = timeQueue.Dequeue();

                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start(); // 重置并启动计时器

                        // Console.WriteLine("flag1=" + stopwatch.ElapsedMilliseconds + "start: " + data.start + ", duration: " + data.duration);

                        // 收集平稳段数据
                        while (stopwatch.ElapsedMilliseconds <= (data.start + data.duration) && isCollectingData)
                        {
                            if (stopwatch.ElapsedMilliseconds >= data.start)
                            {
                                visaClient.Write("CREAD?");

                                byte[] responseBytes = visaClient.Read(10240);
                                
                                // 检查响应是否为空
                                if (responseBytes.Length > 0)
                                {
                                    string result = Encoding.UTF8.GetString(responseBytes);
                                    stableData.Add(ProcessingCReadResult(result));
                                    // 打印响应
                                    Console.WriteLine("Stable Phase Data: " + result);
                                }
                            }
                        }

                        // 收集突变段数据
                        while (isCollectingData)
                        {
                            if (stopwatch.ElapsedMilliseconds >= data.start + data.duration)
                            {
                                visaClient.Write("CREAD?");

                                byte[] responseBytes = visaClient.Read(10240);
                                // 检查响应是否为空
                                if (responseBytes.Length > 0)
                                {
                                    string result = Encoding.UTF8.GetString(responseBytes);
                                    mutationData.Add(ProcessingCReadResult(result));
                                    // 打印响应
                                    Console.WriteLine("Stable Phase Data: " + result);
                                }
                            }
                        }

                        end = DateTime.Now;
                        stopwatch.Stop();

                        double duration = (end - start).TotalSeconds;
                        all.totalDuration = duration;
                        T = duration;

                        List<double> smoothList = ProcessingCurrentData(stableData, (data.duration - data.start)/1000);
                        List<double> mutationList = ProcessingCurrentData(mutationData, (duration - (data.duration/1000)));

                        // 计算平稳段和突变段的平均值
                        double stableAverage = smoothList.Count > 0 ? smoothList.Average() : 0;
                        double mutationAverage = mutationList.Count > 0 ? mutationList.Average() : 0;

                        // 将所有数据都添加到 DataQueue
                        all.stableList = smoothList;
                        all.mutationList = mutationList;
                        all.serialNo = this.currentSerialNo;
                        currentDataQueue.AddData(stableAverage, mutationAverage, all);
                    }
                }
            }
        }

        private List<double> ProcessingCReadResult(string result)
        {
            List<double> dataList = new List<double>();
            string[] dataArray = result.Split(',');
            foreach (string item in dataArray)
            {
                if (double.TryParse(item, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double number))
                {
                    dataList.Add(number * 3000);
                }
                else
                {
                    Console.WriteLine($"无法解析的项: {item}");
                }
            }
            return dataList;
        }

        private List<double> ProcessingCurrentData(List<List<double>> list, double duration)
        {
            List<double> result = new List<double>();
            int fullLists = (int)Math.Floor(duration); // 完整的列表数
            double remainingDuration = duration - fullLists; // 剩余的时间

            // 处理完整的列表
            for (int i = 0; i < fullLists; i++)
            {
                if (i < list.Count)
                {
                    result.AddRange(list[i]);
                }
            }

            // 处理剩余的部分
            if (fullLists < list.Count)
            {
                double ratio = remainingDuration > 0 ? remainingDuration : 1.0;
                int countToExtract = (int)Math.Ceiling(list[fullLists].Count * ratio);
                result.AddRange(list[fullLists].Take(countToExtract));
            }

            // 输出结果或进一步处理
            /*Console.WriteLine("Processed Data:");
            foreach (var data in result)
            {
                Console.WriteLine(data);
            }*/

            return result;
        }

        public async Task TestData()
        {
            string data = "+0.75785933E-01,+0.69955855E-01,+0.63468570E-01,+0.56984855E-01,+0.51826732E-01,+0.46020213E-01,+0.40496662E-01,+0.35133880E-01,+0.29961984E-01,+0.25200677E-01,+0.20796777E-01,+0.17196680E-01,+0.14152681E-01,+0.11376685E-01,+0.90781176E-02,+0.71333496E-02,+0.57513887E-02,+0.46736513E-02,+0.44437471E-02,+0.44176622E-02,+0.43768803E-02,+0.44184967E-02,+0.43966747E-02,+0.43786989E-02,+0.44240714E-02,+0.44672977E-02,+0.53987509E-02,+0.69988710E-02,+0.90120260E-02,+0.11555315E-01,+0.15014704E-01,+0.19384350E-01,+0.24751005E-01,+0.31230573E-01,+0.38753417E-01,+0.47279472E-01,+0.56492015E-01,+0.63402804E-01,+0.68574164E-01,+0.72656760E-01,+0.76214171E-01,+0.79430358E-01,+0.81483641E-01,+0.82857611E-01,+0.83999320E-01,+0.84653769E-01,+0.85163603E-01,+0.85278342E-01,+0.85305564E-01,+0.85327506E-01,+0.85356658E-01,+0.85264899E-01,+0.84959098E-01,+0.84953818E-01,+0.84091468E-01,+0.82844763E-01,+0.81518012E-01,+0.79319045E-01,+0.76456534E-01,+0.72634070E-01,+0.68229247E-01,+0.63132594E-01,+0.57703876E-01,+0.52379909E-01,+0.47087127E-01,+0.42041900E-01,+0.36767211E-01,+0.31487426E-01,+0.26408035E-01,+0.21563196E-01,+0.17604229E-01,+0.14259823E-01,+0.11445579E-01,+0.92653620E-02,+0.72893817E-02,+0.57306997E-02,+0.45307957E-02,+0.35237413E-02,+0.28904606E-02,+0.27479625E-02,+0.26962698E-02,+0.27482012E-02,+0.27205363E-02,+0.27249783E-02,+0.27547894E-02,+0.27422984E-02,+0.27490061E-02,+0.27593506E-02,+0.28200761E-02,+0.33047477E-02,+0.43230711E-02,+0.57391955E-02,+0.72238862E-02,+0.91369354E-02,+0.11247334E-01,+0.13675998E-01,+0.16563637E-01,+0.19806507E-01,+0.23503402E-01,+0.27555589E-01,+0.32448722E-01,+0.38056816E-01,+0.43663066E-01,+0.48278476E-01,+0.52376125E-01,+0.56029105E-01,+0.59508800E-01,+0.62361320E-01,+0.65413817E-01,+0.68241889E-01,+0.70823485E-01,+0.72882758E-01,+0.75572692E-01,+0.78967803E-01,+0.82736304E-01,+0.86393408E-01,+0.90180808E-01,+0.93832121E-01,+0.96938338E-01,+0.10005523E+00,+0.10267361E+00,+0.10486846E+00,+0.10656026E+00,+0.10712130E+00,+0.10718056E+00,+0.10716844E+00,+0.10731120E+00,+0.10721738E+00,+0.10648846E+00,+0.10511781E+00,+0.10406466E+00,+0.10180732E+00,+0.98929045E-01,+0.95500571E-01,+0.91118941E-01,+0.86581603E-01,+0.81733847E-01,+0.76194312E-01,+0.70463843E-01,+0.64553046E-01,+0.58473361E-01,+0.52915858E-01,+0.47351703E-01,+0.42052364E-01,+0.36899810E-01,+0.31947027E-01,+0.27751241E-01,+0.24079774E-01,+0.20827425E-01,+0.18067915E-01,+0.16128601E-01,+0.14609896E-01";

            string data2 = "+0.10333846E+00,+0.10390642E+00,+0.10437658E+00,+0.10450918E+00,+0.10448014E+00,+0.10441843E+00,+0.10345061E+00,+0.10177026E+00,+0.99576369E-01,+0.96864790E-01,+0.93120261E-01,+0.88608176E-01,+0.83676078E-01,+0.78356940E-01,+0.72591056E-01,+0.66870872E-01,+0.61240207E-01,+0.56139499E-01,+0.51360213E-01,+0.47057407E-01,+0.43166171E-01,+0.39365798E-01,+0.35528552E-01,+0.32351924E-01,+0.29838775E-01,+0.27484996E-01,+0.25651991E-01,+0.24221019E-01,+0.23143968E-01,+0.22568372E-01,+0.22403963E-01,+0.22408018E-01,+0.22446771E-01,+0.22394514E-01,+0.22411058E-01,+0.22392189E-01,+0.22381635E-01,+0.22853220E-01,+0.24244064E-01,+0.26319732E-01,+0.28729169E-01,+0.31658634E-01,+0.35098258E-01,+0.39543117E-01,+0.44359607E-01,+0.49671706E-01,+0.57230533E-01,+0.62074267E-01,+0.68254111E-01,+0.73260405E-01,+0.77539725E-01,+0.81523406E-01,+0.85128035E-01,+0.88155074E-01,+0.90424338E-01,+0.91901839E-01,+0.93077208E-01,+0.94148032E-01,+0.94820548E-01,+0.95407019E-01,+0.95956648E-01,+0.96130957E-01,+0.96512930E-01,+0.96579382E-01,+0.96554190E-01,+0.96195563E-01,+0.95106284E-01,+0.93400153E-01,+0.91038268E-01,+0.87751670E-01,+0.83973174E-01,+0.80056662E-01,+0.75462836E-01,+0.70496726E-01,+0.65118384E-01,+0.59753726E-01,+0.54687778E-01,+0.50149787E-01,+0.45883022E-01,+0.42101341E-01,+0.38635005E-01,+0.35242336E-01,+0.32565940E-01,+0.30413446E-01,+0.28589860E-01,+0.27160350E-01,+0.26469833E-01,+0.26005552E-01,+0.25853098E-01,+0.25868093E-01,+0.25820753E-01,+0.25846510E-01,+0.25865112E-01,+0.26227348E-01,+0.26378074E-01,+0.26877502E-01,+0.28521743E-01,+0.30612020E-01,+0.33172897E-01,+0.36358173E-01,+0.40503158E-01,+0.45009639E-01,+0.50327434E-01,+0.56128230E-01,+0.62533896E-01,+0.68363967E-01,+0.73744010E-01,+0.78582373E-01,+0.83581275E-01,+0.87225440E-01,+0.90067916E-01,+0.92372595E-01,+0.94103346E-01,+0.95279875E-01,+0.95999999E-01,+0.96524260E-01,+0.96578040E-01,+0.96591307E-01,+0.96560149E-01,+0.96301238E-01,+0.95173926E-01,+0.93151915E-01,+0.90779387E-01,+0.87443450E-01,+0.83657355E-01,+0.79889777E-01,+0.76088095E-01,+0.72408011E-01,+0.68712577E-01,+0.65135673E-01,+0.61572692E-01,+0.58073863E-01,+0.54918159E-01,+0.52094108E-01,+0.49462787E-01,+0.47146034E-01,+0.45049587E-01,+0.43401618E-01,+0.41981410E-01,+0.41110713E-01,+0.40518154E-01,+0.40037181E-01,+0.40039207E-01,+0.40007278E-01,+0.40025493E-01,+0.40037956E-01,+0.40243892E-01,+0.40619396E-01,+0.41409572E-01,+0.42788551E-01,+0.44462425E-01,+0.46840438E-01";

            List<List<double>> stableData = new List<List<double>>();
            List<List<double>> mutationData = new List<List<double>>();

            stableData.Add(ProcessingCReadResult(data));
            mutationData.Add(ProcessingCReadResult(data2));

            List<double> smoothList = ProcessingCurrentData(stableData, 0.8);
            List<double> mutationList = ProcessingCurrentData(mutationData, 0.5);

            currentDataQueue = new CurrentDataQueue(this.dataGridView1, 20, 3);
            

            CurrentData all = new CurrentData();


            all.totalDuration = 1.5;
            T = 1.5;

            // 计算平稳段和突变段的平均值
            double stableAverage = smoothList.Count > 0 ? smoothList.Average() : 0;
            double mutationAverage = mutationList.Count > 0 ? mutationList.Average() : 0;

            // 将所有数据都添加到 DataQueue
            all.stableList = smoothList;
            all.mutationList = mutationList;
            all.serialNo = 1001;
            currentDataQueue.AddData(stableAverage, mutationAverage, all);
        }

        private void UpdateLEDStatus(Color color)
        {
            this.myLED2.IsFlash = false;
            this.myLED2.LedStatus = true;
            this.myLED2.LedTrueColor = color;
        }
    }

}
