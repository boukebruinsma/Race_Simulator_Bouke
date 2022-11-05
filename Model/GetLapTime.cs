using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class GetLapTime : DataTemplatesInterface
    {
        public string Name { get; set; }
        public Dictionary<Section, TimeSpan> SectionTimes { get; set; }

        public void Add(List<DataTemplatesInterface> dataTemplates)
        {
            
            
            if (dataTemplates.Where(d => d.Name == this.Name).Count() != 0)
            {
                List<GetLapTime> newList = dataTemplates.Cast<GetLapTime>().ToList();
                newList.Find(x => x.Name.Equals(Name)).SectionTimes = SectionTimes;
                dataTemplates = newList.Cast<DataTemplatesInterface>().ToList();
            }
            else
            {
                dataTemplates.Add(this);
            }
        }

        public string FindBestParticipant(List<DataTemplatesInterface> dataTemplates)
        {
            throw new NotImplementedException();
        }
    }
}
