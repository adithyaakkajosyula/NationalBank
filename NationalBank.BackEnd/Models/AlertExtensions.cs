
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NationalBank.BackEnd.Models
{
    public static class AlertExtensions
    {
        private const string _alerts = "_alerts";

        public static List<Alert> GetAlerts(this ITempDataDictionary tempData)
        {
            if (tempData.ContainsKey("_alerts"))
            {
                return DeserializeAlerts(tempData["_alerts"] as string);
            }

            return new List<Alert>();
        }

        public static void AddAlert(this ITempDataDictionary tempData, Alert alert)
        {
            if (alert == null)
            {
                throw new ArgumentNullException("alert");
            }

            List<Alert> alerts = tempData.GetAlerts();
            alerts.Add(alert);
            tempData["_alerts"] = SerializeAlerts(alerts);
        }

        public static void AddAlertInfo(this ITempDataDictionary tempData, string Message)
        {
            tempData.AddAlert(new Alert
            {
                AlertClass = "alert-info",
                Message = Message
            });
        }

        public static void AddAlertSuccess(this ITempDataDictionary tempData, string Message)
        {
            tempData.AddAlert(new Alert
            {
                AlertClass = "alert-success",
                Message = Message
            });
        }

        public static void AddAlertWarning(this ITempDataDictionary tempData, string Message)
        {
            tempData.AddAlert(new Alert
            {
                AlertClass = "alert-warning",
                Message = Message
            });
        }

        public static void AddAlertDanger(this ITempDataDictionary tempData, string Message)
        {
            tempData.AddAlert(new Alert
            {
                AlertClass = "alert-danger",
                Message = Message
            });
        }

        public static void AddAlert(this ITempDataDictionary tempData, params Alert[] alerts)
        {
            if (alerts == null)
            {
                throw new ArgumentNullException("alerts");
            }

            List<Alert> alerts2 = tempData.GetAlerts();
            alerts2.AddRange(alerts);
            tempData["_alerts"] = SerializeAlerts(alerts2);
        }

        private static string SerializeAlerts(List<Alert> Messages)
        {
            return JsonConvert.SerializeObject(Messages);
        }

        private static List<Alert> DeserializeAlerts(string Messages)
        {
            return JsonConvert.DeserializeObject<List<Alert>>(Messages);
        }
    }
}
