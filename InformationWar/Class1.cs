using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace InformationWar
{
    public class Model
    {
        public double N1;
        public double N2;
        public double a;
        public double b;
        public double d;
        public double y;
        public string f1;
        public string f2;
        public double c = 0;
        public double h = 1;

        public List<double> X1 = new List<double>();
        public List<double> X2 = new List<double>();
        public List<double> x1 = new List<double>();
        public List<double> x2 = new List<double>();
        public void calc()
        {
            X1 = new List<double>();
            X2 = new List<double>();
            x1 = new List<double>();
            x2 = new List<double>();
            // Создаем процесс Python
            var processInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "Module_calculation.py", // Путь к вашему Python-скрипту
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processInfo })
            {
                process.Start();

                // Отправляем входные данные в Python
                double[] inputData = { N1,N2,a,b,y,d };

                using (var sw = process.StandardInput)
                {
                    foreach (var value in inputData)
                    {
                        sw.Write(value + " ");
                    }
                    sw.Write(f1 + " "+f2);

                    sw.Close();
                }

                // Получаем результат из Python
                using (var sr = process.StandardOutput)
                {
                    string resultJson = sr.ReadLine();
                    
                    if (resultJson == "Error")
                    {

                    }
                    else
                    {
                        //  Console.WriteLine(resultJson);
                        try
                        {
                            List<List<double>> result = JsonConvert.DeserializeObject<List<List<double>>>(resultJson);
                            int t = 0;
                            while (t < result[0].Count)
                            {
                                X1.Add(result[0][t]);
                                X2.Add(result[1][t]);
                                x1.Add(result[2][t]);
                                x2.Add(result[3][t]);
                                t += 1;

                            }
                        }
                        catch(Exception ex)
                        {
                            return;
                        }
                    }
                }
            
            }

        }

        public double analit_n2(double current_n1)
        {
            return 0;
        }

        public bool wrong_number()
        {
            return false;
        }
    }
}
