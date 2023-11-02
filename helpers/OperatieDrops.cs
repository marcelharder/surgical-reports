namespace surgical_reports.helpers;

public class OperatieDrops
{
    XDocument _doc;
    List<Class_Item> _help = new List<Class_Item>();
    private IWebHostEnvironment _env;


    public OperatieDrops(IWebHostEnvironment env)
    {
        _env = env;
        var content = _env.ContentRootPath;
        var filename = "xml/language_file.xml";
        var test = Path.Combine(content, filename);
        XDocument doc = XDocument.Load($"{test}");
        _doc = doc;
    }

    public async Task<List<string>> getGeneralText(string language)
    {
       var ret = new List<string>();
        await Task.Run(() =>
          {
              IEnumerable<XElement> opa = from el in _doc.Descendants("language")
                                          where (string)el.Attribute("id") == language
                                          select el;
              foreach (XElement el in opa)
              {
                  IEnumerable<XElement> op = from tr in opa.Descendants("generaltext").Elements("items") select tr;
                  foreach (XElement s in op)
                  {
                    ret.Add(s.Value);
                  }
              }
          });
        return ret;
    }

    public async Task<List<Class_Item>> getInotropeOptionsAsync(string language)
    {
        _help.Clear();
        await Task.Run(() =>
          {
              IEnumerable<XElement> opa = from el in _doc.Descendants("language")
                                          where (string)el.Attribute("id") == language
                                          select el;
              foreach (XElement el in opa)
              {
                  IEnumerable<XElement> op = from tr in opa.Descendants("operatie").Elements("inotropica").Elements("items") select tr;
                  _help = getList(op);
              }
          });
        return _help;
    }
    public async Task<List<Class_Item>> getPacemakerOptionsAsync(string language)
    {
        _help.Clear();
        await Task.Run(() =>
          {
              IEnumerable<XElement> opa = from el in _doc.Descendants("language")
                                          where (string)el.Attribute("id") == language
                                          select el;
              foreach (XElement el in opa)
              {
                  IEnumerable<XElement> op = from tr in opa.Descendants("operatie").Elements("pacemaker").Elements("items") select tr;
                  _help = getList(op);
              }
          });

        return _help;
    }
    public async Task<List<Class_Item>> getTypeCardiopleg(string language)
    {

        _help.Clear();
        await Task.Run(() =>
          {
              IEnumerable<XElement> opa = from el in _doc.Descendants("language")
                                          where (string)el.Attribute("id") == language
                                          select el;
              foreach (XElement el in opa)
              {
                  IEnumerable<XElement> op = from tr in opa.Descendants("cpb").Elements("typeCardiopleg").Elements("items") select tr;
                  _help = getList(op);
              }
          });
        return _help;

    }
    public async Task<List<Class_Item>> getCPB_delivery(string language)
    {
        _help.Clear();
        await Task.Run(() =>
          {
              IEnumerable<XElement> opa = from el in _doc.Descendants("language")
                                          where (string)el.Attribute("id") == language
                                          select el;
              foreach (XElement el in opa)
              {
                  IEnumerable<XElement> op = from tr in opa.Descendants("cpb").Elements("delivery").Elements("items") select tr;
                  _help = getList(op);
              }
          });
        return _help;

    }
    public async Task<List<Class_Item>> getCPB_iabp_ind(string language)
    {
        _help.Clear();
        await Task.Run(() =>
          {
              IEnumerable<XElement> opa = from el in _doc.Descendants("language")
                                          where (string)el.Attribute("id") == language
                                          select el;
              foreach (XElement el in opa)
              {
                  IEnumerable<XElement> op = from tr in opa.Descendants("cpb").Elements("iabp_ind").Elements("items") select tr;
                  _help = getList(op);
              }
          });
        return _help;

    }
    public async Task<List<Class_Item>> getCPB_iabp_timing(string language)
    {
        _help.Clear();
        await Task.Run(() =>
          {
              IEnumerable<XElement> opa = from el in _doc.Descendants("language")
                                          where (string)el.Attribute("id") == language
                                          select el;
              foreach (XElement el in opa)
              {
                  IEnumerable<XElement> op = from tr in opa.Descendants("cpb").Elements("iabp_ind_when").Elements("items") select tr;
                  _help = getList(op);
              }
          });
        return _help;

    }
    public async Task<List<Class_Item>> getCPB_temp(string language)
    {
        _help.Clear();
        await Task.Run(() =>
          {
              IEnumerable<XElement> opa = from el in _doc.Descendants("language")
                                          where (string)el.Attribute("id") == language
                                          select el;
              foreach (XElement el in opa)
              {
                  IEnumerable<XElement> op = from tr in opa.Descendants("cpb").Elements("temp").Elements("items") select tr;
                  _help = getList(op);
              }
          });
        return _help;

    }
    public async Task<List<Class_Item>> getCABGRadial(string language)
    {
        _help.Clear();
        await Task.Run(() =>
          {
              IEnumerable<XElement> opa = from el in _doc.Descendants("language")
                                          where (string)el.Attribute("id") == language
                                          select el;
              foreach (XElement el in opa)
              {
                  IEnumerable<XElement> op = from tr in opa.Descendants("cabg").Elements("dropradial").Elements("items") select tr;
                  _help = getList(op);
              }
          });
        return _help;

    }
    public async Task<List<Class_Item>> getCABGLeg(string language)
    {
        _help.Clear();
        await Task.Run(() =>
          {
              IEnumerable<XElement> opa = from el in _doc.Descendants("language")
                                          where (string)el.Attribute("id") == language
                                          select el;
              foreach (XElement el in opa)
              {
                  IEnumerable<XElement> op = from tr in opa.Descendants("cabg").Elements("dropleg").Elements("items") select tr;
                  _help = getList(op);
              }
          });
        return _help;

    }
    private List<Class_Item> getList(IEnumerable<XElement> el)
    {
        foreach (XElement s in el)
        {
            Class_Item _result = new Class_Item();
            _result.description = s.Element("description").Value;
            _result.value = Convert.ToInt32(s.Element("value").Value);
            _help.Add(_result);
        }
        return _help;
    }

}
