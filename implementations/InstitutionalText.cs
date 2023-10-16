
namespace surgical_reports.implementations;

public class InstitutionalText : IInstitutionalText
{
    private XDocument _doc;
    private OperatieDrops _drop;
    private IWebHostEnvironment _env;
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
    OperatieDrops drop)
    {
        _env = env;
        var content = _env.ContentRootPath;
        var filename = "xml/InstitutionalReports.xml";
        var test = Path.Combine(content, filename);
        XDocument doc = XDocument.Load($"{test}");
        _doc = doc;
        _drop = drop;
        _proc = proc;
        _icpb = icpb;
        _cabg = cabg;

    }
    #region <!--institutional stuff-->
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
                await addRecordInXML(hospital);
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
    public string updateInstitutionalReport(InstitutionalDTO rep, int soort, int hospitalNo)
    {

        var contentRoot = _env.ContentRootPath;
        var filename = Path.Combine(contentRoot, "conf/InstitutionalReports.xml");
        XDocument doc = XDocument.Load(filename);
        IEnumerable<XElement> help = from d in doc.Descendants("hospital")
                                     where d.Attribute("id").Value == hospitalNo.ToString().makeSureTwoChar()
                                     select d;
        if (help.Any())
        {
            foreach (XElement original in help)
            {
                IEnumerable<XElement> help2 = from d in original.Elements("reports")
                .Elements("text_by_type_of_surgery")
                .Elements("soort")
                                              where d.Attribute("id").Value == soort.ToString()
                                              select d;
                foreach (XElement f in help2)
                {
                    updateXML(f, rep);
                }
                doc.Save(filename);
            }
        }

        return "";
    }

   


    #endregion

    #region <!--additionalReport stuff-->
    public AdditionalReportDTO getAdditionalReportItems(int hospitalNo, int which)
    {
        var ar = new AdditionalReportDTO();
        var contentRoot = _env.ContentRootPath;
        var filename = Path.Combine(contentRoot, "assets/json/additionalReportItems.json");
        var jsonData = System.IO.File.ReadAllText(filename);
        var oldjson = System.Text.Json.JsonSerializer.Deserialize<List<entities.Root>>(jsonData);
        var selectedARep = oldjson.Find(x => x.hospitalNo == hospitalNo);

        if (selectedARep == null)
        {
            this.createAdditionalReport(hospitalNo);
            jsonData = System.IO.File.ReadAllText(filename);
            oldjson = System.Text.Json.JsonSerializer.Deserialize<List<entities.Root>>(jsonData);
            selectedARep = oldjson.Find(x => x.hospitalNo == hospitalNo);
        }


        switch (which)
        {
            // request circ. support items
            case 1:
                ar.line_1 = selectedARep.circulation_support.items[0].content;
                ar.line_2 = selectedARep.circulation_support.items[1].content;
                ar.line_3 = selectedARep.circulation_support.items[2].content;
                ar.line_4 = selectedARep.circulation_support.items[3].content;
                ar.line_5 = selectedARep.circulation_support.items[4].content;
                break;
            // request iabp. support items
            case 2:
                ar.line_1 = selectedARep.iabp.items[0].content;
                ar.line_2 = selectedARep.iabp.items[1].content;
                ar.line_3 = selectedARep.iabp.items[2].content;
                ar.line_4 = selectedARep.iabp.items[3].content;
                ar.line_5 = selectedARep.iabp.items[4].content;
                break;
            // request pmwires. support items
            case 3:
                ar.line_1 = selectedARep.pmwires.items[0].content;
                ar.line_2 = selectedARep.pmwires.items[1].content;
                ar.line_3 = selectedARep.pmwires.items[2].content;
                ar.line_4 = selectedARep.pmwires.items[3].content;
                ar.line_5 = selectedARep.pmwires.items[4].content;
                break;
        }


        /*  var contentRoot = _env.ContentRootPath;
         var filename = Path.Combine(contentRoot, "conf/InstitutionalReports.xml");
         var doc = XDocument.Load(filename);

         var hospital = doc.Descendants("hospital")
            .FirstOrDefault(h => h.Attribute("id").Value == hospitalNo.ToString().makeSureTwoChar());

         if (hospital != null)
         {
             var reports = hospital.Element("reports");

             switch (which)
             {
                 case 1:
                     var circulationSupport = reports.Element("circulation_support");
                     var items = circulationSupport.Elements("items");
                     foreach (var item in items)
                     {
                         l.Add(item.Element("regel_21").Value);
                     }
                     break;
                 case 2:
                     var iabp = reports.Element("iabp");
                     var iabpItems = iabp.Elements("items");
                     foreach (var item in iabpItems)
                     {
                         l.Add(item.Element("regel_22").Value);
                     }
                     break;
                 case 3:
                     var pmwires = reports.Element("pmwires");
                     var pmwiresItems = pmwires.Elements("items");
                     foreach (var item in pmwiresItems)
                     {
                         l.Add(item.Element("regel_23").Value);
                     }
                     break;
             } */



        return ar;
    }
    public int updateAdditionalReportItem(AdditionalReportDTO up, int hospitalNo, int which)
    {
        up = checkforNullInAdditionalReport(up);
        var contentRoot = _env.ContentRootPath;
        var filename = Path.Combine(contentRoot, "assets/json/additionalReportItems.json");
        var jsonData = System.IO.File.ReadAllText(filename);
        var oldjson = System.Text.Json.JsonSerializer.Deserialize<List<entities.Root>>(jsonData);

        var selectedARep = oldjson.Find(x => x.hospitalNo == hospitalNo);

        switch (which)
        {

            case 1:
                selectedARep.circulation_support.items.RemoveAll(item => item.content != "");
                selectedARep.circulation_support.items.Add(new entities.Item { content = up.line_1 });
                selectedARep.circulation_support.items.Add(new entities.Item { content = up.line_2 });
                selectedARep.circulation_support.items.Add(new entities.Item { content = up.line_3 });
                selectedARep.circulation_support.items.Add(new entities.Item { content = up.line_4 });
                selectedARep.circulation_support.items.Add(new entities.Item { content = up.line_5 });
                break;
            case 2:
                selectedARep.iabp.items.RemoveAll(item => item.content != "");
                selectedARep.iabp.items.Add(new entities.Item { content = up.line_1 });
                selectedARep.iabp.items.Add(new entities.Item { content = up.line_2 });
                selectedARep.iabp.items.Add(new entities.Item { content = up.line_3 });
                selectedARep.iabp.items.Add(new entities.Item { content = up.line_4 });
                selectedARep.iabp.items.Add(new entities.Item { content = up.line_5 });
                break;
            case 3:
                selectedARep.pmwires.items.RemoveAll(item => item.content != "");
                selectedARep.pmwires.items.Add(new entities.Item { content = up.line_1 });
                selectedARep.pmwires.items.Add(new entities.Item { content = up.line_2 });
                selectedARep.pmwires.items.Add(new entities.Item { content = up.line_3 });
                selectedARep.pmwires.items.Add(new entities.Item { content = up.line_4 });
                selectedARep.pmwires.items.Add(new entities.Item { content = up.line_5 });
                break;
        }

        var test_json = System.Text.Json.JsonSerializer.Serialize(oldjson);
        File.WriteAllText(filename, test_json);
        return 1;
    }
    public string createAdditionalReport(int hospitalNo)
    {
        var contentRoot = _env.ContentRootPath;
        var filename = Path.Combine(contentRoot, "assets/json/additionalReportItems.json");
        var jsonData = System.IO.File.ReadAllText(filename);
        var oldjson = System.Text.Json.JsonSerializer.Deserialize<List<entities.Root>>(jsonData);


        var circ = new entities.CirculationSupport
        {
            items = new List<entities.Item>
    {
        new entities.Item { content = "Circ-1" },
        new entities.Item { content = "Circ-2" },
        new entities.Item { content = "Circ-3" },
        new entities.Item { content = "Circ-4" },
        new entities.Item { content = "Circ-5" }
    }
        };

        var iabp = new entities.Iabp
        {
            items = new List<entities.Item>
    {
        new entities.Item { content = "iabp_1" },
        new entities.Item { content = "iabp_2" },
        new entities.Item { content = "iabp_3" },
        new entities.Item { content = "iabp_4" },
        new entities.Item { content = "iabp_5" }
    }
        };

        var pm = new entities.Pmwires
        {
            items = new List<entities.Item>
    {
        new entities.Item { content = "pm_1" },
        new entities.Item { content = "pm_2" },
        new entities.Item { content = "pm_3" },
        new entities.Item { content = "pm_4" },
        new entities.Item { content = "pm_5" }
    }
        };

        var test = new Root
        {
            hospitalNo = hospitalNo,
            circulation_support = circ,
            iabp = iabp,
            pmwires = pm
        };

        oldjson.Add(test);

        var test_json = System.Text.Json.JsonSerializer.Serialize(oldjson);

        File.WriteAllText(filename, test_json);

        // now write this to the json file 
        Console.WriteLine(oldjson);

        return test_json;
    }

    #endregion

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
            var ci = dropRadial.Single(x => x.value == test);
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
        Class_CPB cpb = await _icpb.getSpecificCPB(procedure_id);
        if (cpb != null) { }
        return help;
    }
    private async Task<string> getCardioPlegiaType(int procedure_id)
    {
        var help = "";
        Class_CPB cpb = await _icpb.getSpecificCPB(procedure_id);
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
        result.Add(ad.Element("regel_1_a").Value + "" + ad.Element("regel_1_b").Value + "" + await translateHarvestLocationLeg(procedure_id, dropLeg) + "" + ad.Element("regel_1_c").Value);
        result.Add(ad.Element("regel_2_a").Value + "" + ad.Element("regel_2_b").Value + "" + await translateHarvestLocationRadial(procedure_id, dropRadial) + "" + ad.Element("regel_2_c").Value);
        result.Add(ad.Element("regel_3_a").Value + "" + ad.Element("regel_3_b").Value + "" + ad.Element("regel_3_c").Value);
        result.Add(ad.Element("regel_4_a").Value + "" + ad.Element("regel_4_b").Value + "" + ad.Element("regel_4_c").Value);
        result.Add(ad.Element("regel_5_a").Value + "" + 34 + "" + ad.Element("regel_5_b").Value + "" + ad.Element("regel_5_c").Value);
        result.Add(ad.Element("regel_6_a").Value + "" + ad.Element("regel_6_b").Value +
        "" + await getCardioPlegiaTemp(procedure_id) +
        "" + await getCardioPlegiaRoute(procedure_id) +
        "" + await getCardioPlegiaType(procedure_id) +
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
        result.Add(await getCirculationSupportAsync(procedure_id, test));
        result.Add(await getIABPUsedAsync(procedure_id, test));
        result.Add(await getPMWiresAsync(procedure_id, test));
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
    public async Task addRecordInXML(string id)
    {
        // find out if there is a record with this id
        if (IsNotInXML(id)) // add a node to the xml with hospital_id attribute
        {
            await Task.Run(() =>
        {
            var nodes = _doc.Root.Descendants("hospital");
            IEnumerable<XElement> op = from el in _doc.Descendants("hospital")
                                       where (string)el.Attribute("id") == "01"
                                       select el;
            foreach (XElement el in op)
            {
                XElement nxl = new XElement(el);
                nxl.Attribute("id").SetValue(id);
                _doc.Element("root").Add(nxl);
            }
            var content = _env.ContentRootPath;
            var filename = "xml/InstitutionalReports.xml";
            var test = Path.Combine(content, filename);
            _doc.Save($"{test}");
        }
        );
        }
    }
    private Boolean IsNotInXML(string hospital)
    {
        IEnumerable<XElement> op = from el in _doc.Descendants("hospital")
                                   where (string)el.Attribute("id") == hospital
                                   select el;
        if (op.Count() == 0) { return true; }
        else { return false; }

    }
    private AdditionalReportDTO checkforNullInAdditionalReport(AdditionalReportDTO up)
    {

        up.line_1 = up.line_1 == null ? "" : up.line_1;
        up.line_2 = up.line_2 == null ? "" : up.line_2;
        up.line_3 = up.line_3 == null ? "" : up.line_3;
        up.line_4 = up.line_4 == null ? "" : up.line_4;
        up.line_5 = up.line_5 == null ? "" : up.line_5;
        return up;
    }
    private XElement updateXML(XElement el, InstitutionalDTO rep)
    {
        rep = checkForNullValues(rep);

        el.Element("regel_1_a").SetValue(rep.Regel1A);
        el.Element("regel_1_b").SetValue(rep.Regel1B);
        el.Element("regel_1_c").SetValue(rep.Regel1C);

        el.Element("regel_2_a").SetValue(rep.Regel2A);
        el.Element("regel_2_b").SetValue(rep.Regel2B);
        el.Element("regel_2_c").SetValue(rep.Regel2C);

        el.Element("regel_3_a").SetValue(rep.Regel3A);
        el.Element("regel_3_b").SetValue(rep.Regel3B);
        el.Element("regel_3_c").SetValue(rep.Regel3C);

        el.Element("regel_4_a").SetValue(rep.Regel4A);
        el.Element("regel_4_b").SetValue(rep.Regel4B);
        el.Element("regel_4_c").SetValue(rep.Regel4C);

        el.Element("regel_5_a").SetValue(rep.Regel5A);
        el.Element("regel_5_b").SetValue(rep.Regel5B);
        el.Element("regel_5_c").SetValue(rep.Regel5C);

        el.Element("regel_6_a").SetValue(rep.Regel6A);
        el.Element("regel_6_b").SetValue(rep.Regel6B);
        el.Element("regel_6_c").SetValue(rep.Regel6C);

        el.Element("regel_7_a").SetValue(rep.Regel7A);
        el.Element("regel_7_b").SetValue(rep.Regel7B);
        el.Element("regel_7_c").SetValue(rep.Regel7C);

        el.Element("regel_8_a").SetValue(rep.Regel8A);
        el.Element("regel_8_b").SetValue(rep.Regel8B);
        el.Element("regel_8_c").SetValue(rep.Regel8C);

        el.Element("regel_9_a").SetValue(rep.Regel9A);
        el.Element("regel_9_b").SetValue(rep.Regel9B);
        el.Element("regel_9_c").SetValue(rep.Regel9C);

        el.Element("regel_10_a").SetValue(rep.Regel10A);
        el.Element("regel_10_b").SetValue(rep.Regel10B);
        el.Element("regel_10_c").SetValue(rep.Regel10C);

        el.Element("regel_11_a").SetValue(rep.Regel11A);
        el.Element("regel_11_b").SetValue(rep.Regel11B);
        el.Element("regel_11_c").SetValue(rep.Regel11C);

        el.Element("regel_12_a").SetValue(rep.Regel12A);
        el.Element("regel_12_b").SetValue(rep.Regel12B);
        el.Element("regel_12_c").SetValue(rep.Regel12C);

        el.Element("regel_13_a").SetValue(rep.Regel13A);
        el.Element("regel_13_b").SetValue(rep.Regel13B);
        el.Element("regel_13_c").SetValue(rep.Regel13C);

        el.Element("regel_14_a").SetValue(rep.Regel14A);
        el.Element("regel_14_b").SetValue(rep.Regel14B);
        el.Element("regel_14_c").SetValue(rep.Regel14C);

        el.Element("regel_15").SetValue(rep.Regel15);
        el.Element("regel_16").SetValue(rep.Regel16);
        el.Element("regel_17").SetValue(rep.Regel17);
        el.Element("regel_18").SetValue(rep.Regel18);
        el.Element("regel_19").SetValue(rep.Regel19);

        el.Element("regel_20").SetValue(rep.Regel20);
        el.Element("regel_21").SetValue(rep.Regel21);
        el.Element("regel_22").SetValue(rep.Regel22);
        el.Element("regel_23").SetValue(rep.Regel23);
        el.Element("regel_24").SetValue(rep.Regel24);
        el.Element("regel_25").SetValue(rep.Regel25);
        el.Element("regel_26").SetValue(rep.Regel26);
        el.Element("regel_27").SetValue(rep.Regel27);
        el.Element("regel_28").SetValue(rep.Regel28);
        el.Element("regel_29").SetValue(rep.Regel29);
        el.Element("regel_30").SetValue(rep.Regel30);
        el.Element("regel_31").SetValue(rep.Regel31);
        el.Element("regel_32").SetValue(rep.Regel32);
        el.Element("regel_33").SetValue(rep.Regel33);
        return el;
    }
    private InstitutionalDTO checkForNullValues(InstitutionalDTO test)
    {
        test.Regel1A = test.Regel1A == null ? "" : test.Regel1A;
        test.Regel1B = test.Regel1B == null ? "" : test.Regel1B;
        test.Regel1C = test.Regel1C == null ? "" : test.Regel1C;

        test.Regel2A = test.Regel2A == null ? "" : test.Regel2A;
        test.Regel2B = test.Regel2B == null ? "" : test.Regel2B;
        test.Regel2C = test.Regel2C == null ? "" : test.Regel2C;

        test.Regel3A = test.Regel3A == null ? "" : test.Regel3A;
        test.Regel3B = test.Regel3B == null ? "" : test.Regel3B;
        test.Regel3C = test.Regel3C == null ? "" : test.Regel3C;

        test.Regel4A = test.Regel4A == null ? "" : test.Regel4A;
        test.Regel4B = test.Regel4B == null ? "" : test.Regel4B;
        test.Regel4C = test.Regel4C == null ? "" : test.Regel4C;

        test.Regel5A = test.Regel5A == null ? "" : test.Regel5A;
        test.Regel5B = test.Regel5B == null ? "" : test.Regel5B;
        test.Regel5C = test.Regel5C == null ? "" : test.Regel5C;

        test.Regel6A = test.Regel6A == null ? "" : test.Regel6A;
        test.Regel6B = test.Regel6B == null ? "" : test.Regel6B;
        test.Regel6C = test.Regel6C == null ? "" : test.Regel6C;

        test.Regel7A = test.Regel7A == null ? "" : test.Regel7A;
        test.Regel7B = test.Regel7B == null ? "" : test.Regel7B;
        test.Regel7C = test.Regel7C == null ? "" : test.Regel7C;

        test.Regel8A = test.Regel8A == null ? "" : test.Regel8A;
        test.Regel8B = test.Regel8B == null ? "" : test.Regel8B;
        test.Regel8C = test.Regel8C == null ? "" : test.Regel8C;

        test.Regel9A = test.Regel9A == null ? "" : test.Regel9A;
        test.Regel9B = test.Regel9B == null ? "" : test.Regel9B;
        test.Regel9C = test.Regel9C == null ? "" : test.Regel9C;

        test.Regel10A = test.Regel10A == null ? "" : test.Regel10A;
        test.Regel10B = test.Regel10B == null ? "" : test.Regel10B;
        test.Regel10C = test.Regel10C == null ? "" : test.Regel10C;

        test.Regel11A = test.Regel11A == null ? "" : test.Regel11A;
        test.Regel11B = test.Regel11B == null ? "" : test.Regel11B;
        test.Regel11C = test.Regel11C == null ? "" : test.Regel11C;

        test.Regel12A = test.Regel12A == null ? "" : test.Regel12A;
        test.Regel12B = test.Regel12B == null ? "" : test.Regel12B;
        test.Regel12C = test.Regel12C == null ? "" : test.Regel12C;

        test.Regel13A = test.Regel13A == null ? "" : test.Regel13A;
        test.Regel13B = test.Regel13B == null ? "" : test.Regel13B;
        test.Regel13C = test.Regel13C == null ? "" : test.Regel13C;

        test.Regel14A = test.Regel14A == null ? "" : test.Regel14A;
        test.Regel14B = test.Regel14B == null ? "" : test.Regel14B;
        test.Regel14C = test.Regel14C == null ? "" : test.Regel14C;

        test.Regel15 = test.Regel15 == null ? "" : test.Regel15;
        test.Regel16 = test.Regel16 == null ? "" : test.Regel16;
        test.Regel17 = test.Regel17 == null ? "" : test.Regel17;
        test.Regel18 = test.Regel18 == null ? "" : test.Regel18;
        test.Regel19 = test.Regel19 == null ? "" : test.Regel19;
        test.Regel20 = test.Regel20 == null ? "" : test.Regel20;

        test.Regel21 = test.Regel21 == null ? "" : test.Regel21;
        test.Regel22 = test.Regel22 == null ? "" : test.Regel22;
        test.Regel23 = test.Regel23 == null ? "" : test.Regel23;
        test.Regel24 = test.Regel24 == null ? "" : test.Regel24;
        test.Regel25 = test.Regel25 == null ? "" : test.Regel25;
        test.Regel26 = test.Regel26 == null ? "" : test.Regel26;
        test.Regel27 = test.Regel27 == null ? "" : test.Regel27;
        test.Regel28 = test.Regel28 == null ? "" : test.Regel28;
        test.Regel29 = test.Regel29 == null ? "" : test.Regel29;
        test.Regel30 = test.Regel30 == null ? "" : test.Regel30;
        test.Regel31 = test.Regel31 == null ? "" : test.Regel31;
        test.Regel32 = test.Regel32 == null ? "" : test.Regel32;
        test.Regel33 = test.Regel33 == null ? "" : test.Regel33;


        return test;
    }

}
