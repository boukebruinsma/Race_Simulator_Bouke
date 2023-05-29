using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class GetPoints : DataTemplatesInterface
    {
        public GetPoints(string Name, int ScoredPoints)
        {
            this.Name = Name;
            this.ScoredPoints = ScoredPoints;
        }
        public string Name { get; set; }
        public int ScoredPoints { get; set; }

        public void Add(List<DataTemplatesInterface> dataTemplates)
        {
            if (dataTemplates.Where(d => d.Name == this.Name).Count() != 0)
            {
                List<GetPoints> newList = dataTemplates.Cast<GetPoints>().ToList();
                newList.Find(x => x.Name.Equals(Name)).ScoredPoints += ScoredPoints;
                dataTemplates = newList.Cast<DataTemplatesInterface>().ToList();
            }
            else
            {
                dataTemplates.Add(this);
            }
        }

        public string FindBestParticipant(List<DataTemplatesInterface> dataTemplates)
        {
            try
            {
                
                List<GetPoints> newList = dataTemplates.Cast<GetPoints>().ToList();
                return newList.Where(d => d.ScoredPoints == newList.Max(x => x.ScoredPoints)).First().Name;
            }
            catch
            {
                return "leeg";
            }
        }
    }
}
