
namespace surgical_reports.implementations;

public class InstitutionalText : IInstitutionalText
{
    private XDocument _doc;
    private XElement _element;
    private reportMapper _sm;
    private OperatieDrops _drop;
    private IWebHostEnvironment _env;
    private DapperContext _context;
    private List<Class_Item> dropRadial = new List<Class_Item>();
    private List<Class_Item> dropLeg = new List<Class_Item>();
    private IProcedureRepository _proc;
    private ICPBRepo _icpb;
    private ICABGRepo _cabg;

    public InstitutionalText(
        ICABGRepo cabg,
    ICPBRepo icpb,
    IProcedureRepository proc,
    IWebHostEnvironment env,
    DapperContext context,
    OperatieDrops drop,
    reportMapper sm)
    {
        _env = env;
        _sm = sm;
        var content = _env.ContentRootPath;
        var filename = "conf/InstitutionalReports.xml";
        var test = Path.Combine(content, filename);
        XDocument doc = XDocument.Load($"{test}");
        _doc = doc;
        XElement element = XElement.Load($"{test}");
        _element = element;
        _context = context;
        _drop = drop;
        _proc = proc;
        _icpb = icpb;

    }
    public async Task<List<string>> getText(string hospital, string soort, int procedure_id)
    {
        hospital = hospital.makeSureTwoChar();
        dropRadial = await _drop.getCABGRadial();
        dropLeg = await _drop.getCABGLeg();
        var result = new List<string>();
        await Task.Run(async () =>
        {
            // get the correct hospital
            IEnumerable<XElement> op = from el in _doc.Descendants("hospital")
                                       where (string)el.Attribute("id") == hospital
                                       select el;
            if (op.Count() == 0)
            {
                //add a new Element for this hospital to the XML file
                addElementToInstitutionalXML(hospital);
            }

            foreach (XElement el in op)
            {
                IEnumerable<XElement> t = from tr in op.Descendants("reports").Elements("text_by_type_of_surgery").Elements("soort")
                                          where (string)tr.Attribute("id") == soort
                                          select tr;
                if (t.Count() == 0)
                { // no institutional record found so come up with a new record now
                  // get description from fdType
                    var description = await getProcedureDescriptionAsync(procedure_id);
                    result = this.getEmptyRecord(description);

                }
                else
                {  // there is a institutional record for this soort of procedure
                    foreach (XElement ad in t)
                    {
                        result = await this.getExitingRecordAsync(ad, procedure_id, op);
                    }
                }
            }

        });
        return result;
    }
    private async Task<string> translateHarvestLocationLeg(int procedure_id, List<Class_Item> dropLeg)
    {
        var help = "";
        var cabg = await _cabg.getSpecificCABG(procedure_id);

        if (cabg != null && cabg.leg_harvest_location == "")
        {
            var test = Convert.ToInt32(cabg.leg_harvest_location);
            var ci = dropLeg.Single(x => x.value == test);
            help = ci.description;
        }

        return help;
    }
    private async Task<string> translateHarvestLocationRadial(int procedure_id, List<Class_Item> dropRadial)
    {
        var help = "";
        var cabg = await _cabg.getSpecificCABG(procedure_id);
        if (cabg != null && cabg.radial_harvest_location == "")
        {
            var test = Convert.ToInt32(cabg.radial_harvest_location);
            var ci = dropLeg.Single(x => x.value == test);
            help = ci.description;
        }

        return help;
    }
    private async Task<string> getCardioPlegiaTemp(int procedure_id)
    {
        var help = "";
        Class_CPB cpb = await _icpb.getSpecificCPB(procedure_id);
        if (cpb != null) { }
        return help;
    }
    private async Task<string> getCardioPlegiaRoute(int procedure_id)
    {
        var help = "";
        Class_CPB cpb =  await _icpb.getSpecificCPB(procedure_id);
        if (cpb != null) { }
        return help;
    }
    private async Task<string> getCardioPlegiaType(int procedure_id)
    {
        var help = "";
        Class_CPB cpb =  await _icpb.getSpecificCPB(procedure_id);
        if (cpb != null) { }
        return help;
    }
    private async Task<string> getProcedureDescriptionAsync(int procedure_id)
    {
        var r = await _proc.getSpecificProcedure(procedure_id);
        return r.Description;
    }
    private async Task<string> getCirculationSupportAsync(int procedure_id, IEnumerable<XElement> test)
    {
        var help = "";
        var selectedProcedure = await _proc.getSpecificProcedure(procedure_id);
        if (selectedProcedure != null)
        {
            var t = selectedProcedure.SelectedInotropes; // dit is de gekozen inotropische ondersteuning
            foreach (XElement el in test)// dit is het correcte ziekenhuis, dus ook de juiste taal
            {
                IEnumerable<XElement> te = from tr in test.Descendants("reports").Elements("circulation_support").Elements("items")
                                           where (string)tr.Attribute("id") == t.ToString()
                                           select tr;
                foreach (XElement f in te) { help = f.Element("regel_21").Value; }
            }
        }
        return help;
    }
    private async Task<string> getPMWiresAsync(int procedure_id, IEnumerable<XElement> test)
    {
        var help = "";
        var selectedProcedure = await _proc.getSpecificProcedure(procedure_id);
        if (selectedProcedure != null)
        {
            var t = selectedProcedure.SelectedInotropes; // dit is de gekozen inotropische ondersteuning
            foreach (XElement el in test)// dit is het correcte ziekenhuis, dus ook de juiste taal
            {
                IEnumerable<XElement> te = from tr in test.Descendants("reports").Elements("pmwires").Elements("items")
                                           where (string)tr.Attribute("id") == t.ToString()
                                           select tr;
                foreach (XElement f in te) { help = f.Element("regel_23").Value; }
            }
        }
        return help;
    }
    private async Task<string> getIABPUsedAsync(int procedure_id, IEnumerable<XElement> test)
    {
        var help = "";
        var selectedCPB = await _icpb.getSpecificCPB(procedure_id);
        if (selectedCPB != null)
        {
            var t = selectedCPB.IABP_IND; // dit is de gekozen indicatie voor de IABP
            foreach (XElement el in test)// dit is het correcte ziekenhuis, dus ook de juiste taal
            {
                IEnumerable<XElement> te = from tr in test.Descendants("reports").Elements("iabp").Elements("items")
                                           where (string)tr.Attribute("id") == t.ToString()
                                           select tr;
                foreach (XElement f in te) { help = f.Element("regel_22").Value; }
            }
        }
        return help;
    }
    private List<string> getEmptyRecord(string description)
    {
        var result = new List<string>();
        result.Add("No institutional text for: " + description);
        result.Add("Please enter your custom report here and 'Save as suggestion'");
        for (int x = 2; x < 34; x++) { result.Add(""); }
        return result;
    }
    private async Task<List<string>> getExitingRecordAsync(XElement ad, int procedure_id, IEnumerable<XElement> test)
    {
        var result = new List<string>();
        result.Add(ad.Element("regel_1_a").Value + "" + ad.Element("regel_1_b").Value + "" + this.translateHarvestLocationLeg(procedure_id, dropLeg) + "" + ad.Element("regel_1_c").Value);
        result.Add(ad.Element("regel_2_a").Value + "" + ad.Element("regel_2_b").Value + "" + this.translateHarvestLocationRadial(procedure_id, dropRadial) + "" + ad.Element("regel_2_c").Value);
        result.Add(ad.Element("regel_3_a").Value + "" + ad.Element("regel_3_b").Value + "" + ad.Element("regel_3_c").Value);
        result.Add(ad.Element("regel_4_a").Value + "" + ad.Element("regel_4_b").Value + "" + ad.Element("regel_4_c").Value);
        result.Add(ad.Element("regel_5_a").Value + "" + 34 + "" + ad.Element("regel_5_b").Value + "" + ad.Element("regel_5_c").Value);
        result.Add(ad.Element("regel_6_a").Value + "" + ad.Element("regel_6_b").Value +
        "" + this.getCardioPlegiaTemp(procedure_id) +
        "" + this.getCardioPlegiaRoute(procedure_id) +
        "" + this.getCardioPlegiaType(procedure_id) +
        "" + ad.Element("regel_6_c").Value);
        result.Add(ad.Element("regel_7_a").Value + "" + ad.Element("regel_7_b").Value + "" + ad.Element("regel_7_c").Value);
        result.Add(ad.Element("regel_8_a").Value + "" + ad.Element("regel_8_b").Value + "" + ad.Element("regel_8_c").Value);
        result.Add(ad.Element("regel_9_a").Value + "" + ad.Element("regel_9_b").Value + "" + ad.Element("regel_9_c").Value);
        result.Add(ad.Element("regel_10_a").Value + "" + ad.Element("regel_10_b").Value + "" + ad.Element("regel_10_c").Value);
        result.Add(ad.Element("regel_11_a").Value + "" + ad.Element("regel_11_b").Value + "" + ad.Element("regel_11_c").Value);
        result.Add(ad.Element("regel_12_a").Value + "" + ad.Element("regel_12_b").Value + "" + ad.Element("regel_12_c").Value);
        result.Add(ad.Element("regel_13_a").Value + "" + ad.Element("regel_13_b").Value + "" + ad.Element("regel_13_c").Value);
        result.Add(ad.Element("regel_14_a").Value + "" + ad.Element("regel_14_b").Value + "" + ad.Element("regel_14_c").Value);
        result.Add(ad.Element("regel_15").Value);
        result.Add(ad.Element("regel_16").Value);
        result.Add(ad.Element("regel_17").Value);
        result.Add(ad.Element("regel_18").Value);
        result.Add(ad.Element("regel_19").Value);
        result.Add(ad.Element("regel_20").Value);
        result.Add(await this.getCirculationSupportAsync(procedure_id, test));
        result.Add(await this.getIABPUsedAsync(procedure_id, test));
        result.Add(await this.getPMWiresAsync(procedure_id, test));
        result.Add(ad.Element("regel_24").Value);
        result.Add(ad.Element("regel_25").Value);
        result.Add(ad.Element("regel_26").Value);
        result.Add(ad.Element("regel_27").Value);
        result.Add(ad.Element("regel_28").Value);
        result.Add(ad.Element("regel_29").Value);
        result.Add(ad.Element("regel_30").Value);
        result.Add(ad.Element("regel_31").Value);
        result.Add(ad.Element("regel_32").Value);
        result.Add(ad.Element("regel_33").Value);
        return result;
    }
    private void addElementToInstitutionalXML(string hospital)
    {
        var content = _env.ContentRootPath;
        var filename = "conf/InstitutionalReports.xml";
        var test = Path.Combine(content, filename);
        IEnumerable<XElement> op = from el in _doc.Descendants("hospital")
                                   where (string)el.Attribute("id") == "99999"
                                   select el;
        foreach (XElement el in op)
        {
            XElement help = el;
            _doc.Add(help);
            IEnumerable<XElement> op1 = from element in _doc.Descendants("hospital") select el;
            op1.Last().SetAttributeValue("id", hospital);
            _doc.Save($"{test}");
        }



    }
}
