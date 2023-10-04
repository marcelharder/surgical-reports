namespace surgical_reports.helpers;

public class OperatieDrops
    {
        XElement _testje;
        
        private IWebHostEnvironment _env;
         List<Class_Item> _help = new List<Class_Item>();
     

        public OperatieDrops(IWebHostEnvironment env)     
        {
            _env = env;
            var content = _env.ContentRootPath;
            var filename = "xml/language_file.xml";
            var test = Path.Combine(content, filename);
            XElement testje = XElement.Load($"{test}");
            _testje = testje;
            
        }

       
       

        #region <!--operatie-->
        public List<Class_Item> getTimingOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("operatie_timing").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getUrgentOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("urgent_reasons").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getEmergentOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("emergent_reasons").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getInotropeOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("inotropica").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getPericardOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("pericard").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getPleuraOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("pleura").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getPacemakerOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("pacemaker").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getArray(int id)
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("category").Elements("items");
            foreach (XElement s in op)
            {
                if (s.Element("cat").Value == id.ToString())
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            }
            _help.RemoveAt(0); // remove the Choose item for this list
            return _help;
        }

        #endregion

        #region <!--cpb-->
        public async Task<List<Class_Item>> getTypeCardiopleg()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("typeCardiopleg").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getMPT()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("myocardial_protection").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_art_choice()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("art_choice").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_ven_choice()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("ven_choice").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_delivery()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("delivery").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_iabp_ind()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("iabp_ind").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_iabp_timing()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("iabp_ind_when").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_nccp()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("nccp").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_aox()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("aox").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_timing()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("timing").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_temp()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("temp").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        #endregion

         
      
        private List<Class_Item> getGeneralDrops(List<int> list)
        {
            foreach (int h in list)
            {
                Class_Item _result = new Class_Item();
                _result.description = h.ToString();
                _result.value = h;
                _help.Add(_result);
            }
            return _help;
        }
       
    }   
        



   
