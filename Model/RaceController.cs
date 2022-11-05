using System;
using System.Collections.Generic;

namespace Model
{
    public class RaceController<T> where T : DataTemplatesInterface
    {
        public List<DataTemplatesInterface> _list = new List<DataTemplatesInterface>();

        public void PutList(T item)
        {
            item.Add(_list);
        }

        public string FindBestParticipant()
        {
            if(_list != null)
            {
                return _list[0].FindBestParticipant(_list);
            }
            return "";
            
        }
    }
}
