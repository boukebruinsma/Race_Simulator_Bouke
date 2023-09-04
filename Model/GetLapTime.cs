using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class GetLapTime : DataTemplatesInterface
    {
        public string Name { get; set; }
        public Dictionary<Section, TimeSpan> SectionTimes { get; set; }
        public List<double> LapTimes { get; set; }

        public void Add(List<DataTemplatesInterface> dataTemplates)
        {      
            if (dataTemplates.Where(d => d.Name == this.Name).Count() != 0)
            {
                List<GetLapTime> newList = dataTemplates.Cast<GetLapTime>().ToList();
                newList.Find(x => x.Name.Equals(Name)).SectionTimes = SectionTimes;
                newList.Find(x => x.Name.Equals(Name)).LapTimes = LapTimes;
                dataTemplates = newList.Cast<DataTemplatesInterface>().ToList();
            }
            else
            {
                dataTemplates.Add(this);
            }
        }

        //returnt snelste rondetijd
        public string FindBestParticipant(List<DataTemplatesInterface> dataTemplates)
        {
            List<GetLapTime> newList = dataTemplates.Cast<GetLapTime>().ToList();
            Dictionary<String, double> lapRecords = new Dictionary<String, double>();

            foreach (var item in newList)
            {
                lapRecords.Add(item.Name, item.LapTimes.Max());
            }

            double fastestLap = lapRecords.Values.Min();
            return $"{lapRecords.Where(i => i.Value.Equals(fastestLap)).First().Key}: {fastestLap}";
        }
    }
}
