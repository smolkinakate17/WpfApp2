using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp2.ViewModels.Base;
using WpfApp2.Infrostructure.Commands;
using WpfApp2.Models;
using System.Text.Json;
using System.IO;
using System.Reflection;

namespace WpfApp2.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region private fields
        private string _num1="";
        private string _num2 = "";
        private string _result = "";
        private List<MainModelForJson> info=MainModelForJson.getListFromJson("saving.json");
        #endregion

        #region public fields
        public string Num1
        {
            get => _num1;
            set => Set(ref _num1, value);
        }

        public string Num2
        {
            get => _num2;
            set => Set(ref _num2, value);
        }

        public string Result
        {
            get => _result;
            set => Set(ref _result, value);
        }
        public List<MainModelForJson> Info
        {
            get { return info; }
            set => Set(ref info, value);
        }
        #endregion

        #region commands
        public ICommand SumCommand { get; }
        public ICommand SaveCommand { get; }
       
        #region CanDoCommandExecute
        private bool CanSumCommandExecute(object p)
        {
            if (Num1.Equals("") || Num2.Equals(""))
            {
                return false;
            }
            return true;
        }
        private bool CanSaveCommandExecute(object p)
        {
            if (Result.Equals(""))
            {
                return false;
            }
            return true;
        }

        #endregion
        #region OnDoCommandExecute
        private void OnSumCommandExecute(object p)
        {
            try
            {
                Result = (int.Parse(Num1) + int.Parse(Num2)).ToString();
            }
            catch(Exception e)
            {
                try
                {
                    Result = (double.Parse(Num1) + double.Parse(Num2)).ToString();
                }
                catch(Exception ex)
                {
                    Result = "Нельзя посчитать сумму";
                }
            }
        }

        private void OnSaveCommandExecute(object p)
        {
            var toJson = new MainModelForJson
            {
                Number1 = Num1,
                Number2 = Num2,
                Result = Result,
                Date = DateTime.Now,
                BadREsult=Result.Equals("Нельзя посчитать сумму")
                
            };
            string fileName = "saving.json";
            string jsonString = toJson.ToString();
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var relativePath = @"saving.json";
            var fullPath = Path.Combine(appDir, relativePath);
            if (!File.Exists(fullPath))
            {
                FileStream fs = File.Create(fullPath);
                fs.Close();
            }
            

            string readFile = File.ReadAllText(fileName);
            if (readFile.Length == 0)
            {
                File.AppendAllText(fileName, "[\n" + jsonString + "\n]");
            }
            else
            {
                string newFile = readFile.Substring(0,readFile.Length-2);
                newFile += ",\n" + jsonString + "\n]";
                File.WriteAllText(fileName, newFile);
            }
            
            Info= MainModelForJson.getListFromJson("saving.json");
        }
        #endregion
        #endregion
        public MainWindowViewModel()
        {
            SumCommand = new LambdaCommand(OnSumCommandExecute, CanSumCommandExecute);
            SaveCommand = new LambdaCommand(OnSaveCommandExecute, CanSaveCommandExecute);
            

        }

    }
}
