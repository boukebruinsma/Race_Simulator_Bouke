using System;
using System.Collections.Generic;

namespace Model
{
    public interface DataTemplatesInterface
    {
        public string Name { get; set; }


        void Add(List<DataTemplatesInterface> dataTemplates);

        string FindBestParticipant(List<DataTemplatesInterface> dataTemplates);

    }
}
