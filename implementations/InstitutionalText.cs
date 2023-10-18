
using System.Net.WebSockets;
using System.Resources;

namespace surgical_reports.implementations;

public class InstitutionalText : IInstitutionalText
{
    private XDocument _doc;
    private IWebHostEnvironment _env;
    private List<Class_Item> dropLeg = new List<Class_Item>();
    private IProcedureRepository _proc;
    private ICPBRepo _icpb;
    private ICABGRepo _cabg;

    public InstitutionalText(
    ICABGRepo cabg,
    ICPBRepo icpb,
    IProcedureRepository proc,
    IWebHostEnvironment env)
    {
        _env = env;
        var content = _env.ContentRootPath;
        var filename = "xml/InstitutionalReports.xml";
        var test = Path.Combine(content, filename);
        XDocument doc = XDocument.Load($"{test}");
        _doc = doc;
        _proc = proc;
        _icpb = icpb;
        _cabg = cabg;

    }

    public async Task<InstitutionalDTO> getInstitutionalReport(string hospital, string soort, string description)
    {
        hospital = hospital.makeSureTwoChar();
        //dropRadial = await _drop.getCABGRadial();
        //dropLeg = await _drop.getCABGLeg();
        var result = new InstitutionalDTO();
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
                  // var description = await getProcedureDescriptionAsync(procedure_id);
                    result = this.getEmptyRecord(description);
                    // now save this to the xml file again
                    await addXelementtoXML(hospital, soort, result);
                }
                else
                {  // there is a institutional record for this soort of procedure
                    foreach (XElement ad in t)
                    {

                        result = await getExitingRecordAsync(ad);
                    }
                }
            }
        });
        return result;
    }
    public string updateInstitutionalReport(InstitutionalDTO rep, int soort, int hospitalNo)
    {
        var contentRoot = _env.ContentRootPath;
        var filename = Path.Combine(contentRoot, "xml/InstitutionalReports.xml");
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


    private string createAdditionalReport(int hospitalNo)
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
    private InstitutionalDTO getEmptyRecord(string description)
    {
        var result = new InstitutionalDTO();
        result.Regel1A = "No institutional text for: " + description;
        result.Regel2A = "Please enter your custom report here and 'Save as suggestion'";
        return result;
    }
    private async Task<InstitutionalDTO> getExitingRecordAsync(XElement ad)
    {
        var result = new InstitutionalDTO();
        await Task.Run(() =>
        {

            result.Regel1A = ad.Element("regel_1_a").Value;
            result.Regel1B = ad.Element("regel_1_b").Value;
            result.Regel1C = ad.Element("regel_1_c").Value;
            // result.Regel1C = await translateHarvestLocationLeg(procedure_id, dropLeg);

            result.Regel2A = ad.Element("regel_2_a").Value;
            result.Regel2B = ad.Element("regel_2_b").Value;
            result.Regel2C = ad.Element("regel_2_c").Value;
            // result.Regel2C = await translateHarvestLocationRadial(procedure_id, dropLeg);

            result.Regel3A = ad.Element("regel_3_a").Value;
            result.Regel3B = ad.Element("regel_3_b").Value;
            result.Regel3C = ad.Element("regel_3_c").Value;

            result.Regel4A = ad.Element("regel_4_a").Value;
            result.Regel4B = ad.Element("regel_4_b").Value;
            result.Regel4C = ad.Element("regel_4_c").Value;

            result.Regel5A = ad.Element("regel_5_a").Value;
            result.Regel5B = ad.Element("regel_5_b").Value;
            result.Regel5C = ad.Element("regel_5_c").Value;

            result.Regel6A = ad.Element("regel_6_a").Value;
            result.Regel6B = ad.Element("regel_6_b").Value;
            // result.Regel6B = ad.Element("regel_6_b").Value + "" + await getCardioPlegiaTemp(procedure_id) + "" + await getCardioPlegiaRoute(procedure_id) + "" + await getCardioPlegiaType(procedure_id);
            result.Regel6C = ad.Element("regel_6_c").Value;

            result.Regel7A = ad.Element("regel_7_a").Value;
            result.Regel7B = ad.Element("regel_7_b").Value;
            result.Regel7C = ad.Element("regel_7_c").Value;

            result.Regel8A = ad.Element("regel_8_a").Value;
            result.Regel8B = ad.Element("regel_8_b").Value;
            result.Regel8C = ad.Element("regel_8_c").Value;

            result.Regel9A = ad.Element("regel_9_a").Value;
            result.Regel9B = ad.Element("regel_9_b").Value;
            result.Regel9C = ad.Element("regel_9_c").Value;

            result.Regel10A = ad.Element("regel_10_a").Value;
            result.Regel10B = ad.Element("regel_10_b").Value;
            result.Regel10C = ad.Element("regel_10_c").Value;

            result.Regel11A = ad.Element("regel_11_a").Value;
            result.Regel11B = ad.Element("regel_11_b").Value;
            result.Regel11C = ad.Element("regel_11_c").Value;

            result.Regel12A = ad.Element("regel_12_a").Value;
            result.Regel12B = ad.Element("regel_12_b").Value;
            result.Regel12C = ad.Element("regel_12_c").Value;

            result.Regel13A = ad.Element("regel_13_a").Value;
            result.Regel13B = ad.Element("regel_13_b").Value;
            result.Regel13C = ad.Element("regel_13_c").Value;

            result.Regel14A = ad.Element("regel_14_a").Value;
            result.Regel14B = ad.Element("regel_14_b").Value;
            result.Regel14C = ad.Element("regel_14_c").Value;


            result.Regel15 = ad.Element("regel_15").Value;
            result.Regel16 = ad.Element("regel_16").Value;
            result.Regel17 = ad.Element("regel_17").Value;
            result.Regel18 = ad.Element("regel_18").Value;
            result.Regel19 = ad.Element("regel_19").Value;
            result.Regel20 = ad.Element("regel_20").Value;
            result.Regel21 = ad.Element("regel_21").Value;
            result.Regel22 = ad.Element("regel_22").Value;
            result.Regel23 = ad.Element("regel_23").Value;

            // result.Regel21 = await getCirculationSupportAsync(procedure_id, test);
            // result.Regel22 = await getIABPUsedAsync(procedure_id, test);
            // result.Regel23 = await getPMWiresAsync(procedure_id, test);
            result.Regel24 = ad.Element("regel_24").Value;
            result.Regel25 = ad.Element("regel_25").Value;
            result.Regel26 = ad.Element("regel_26").Value;
            result.Regel27 = ad.Element("regel_27").Value;
            result.Regel28 = ad.Element("regel_28").Value;
            result.Regel29 = ad.Element("regel_29").Value;
            result.Regel30 = ad.Element("regel_30").Value;
            result.Regel31 = ad.Element("regel_31").Value;
            result.Regel32 = ad.Element("regel_32").Value;
            result.Regel33 = ad.Element("regel_33").Value;
        });

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
    private async Task addXelementtoXML(string hospital, string soort, InstitutionalDTO result)
    {
        await Task.Run(() =>
        {
            IEnumerable<XElement> op = from el in _doc.Descendants("hospital")
                                       where (string)el.Attribute("id") == hospital
                                       select el;
            foreach (XElement el in op)
            {
                IEnumerable<XElement> t = from tr in op.Descendants("reports").Elements("text_by_type_of_surgery").Elements("soort")
                                          where (string)tr.Attribute("id") == "1"
                                          select tr;
                foreach (XElement top in t)
                {
                    XElement nxl = new XElement(top);
                    nxl = updateXML(nxl, result);
                    nxl.Attribute("id").SetValue(soort);
                    addNewElement(nxl, hospital);
                }
            }
        });
    }
    private void addNewElement(XElement nxl, string hospital)
    {
        XElement help = _doc.Descendants("hospital")
        .Where(x => (string)x.Attribute("id") == hospital)
        .Elements("reports").Elements("text_by_type_of_surgery")
        .FirstOrDefault();

        help.Add(nxl);
        var content = _env.ContentRootPath;
        var filename = "xml/InstitutionalReports.xml";
        var test = Path.Combine(content, filename);
        _doc.Save($"{test}");
    }

    public async Task<IEnumerable<XElement>> getCurrentHospital(string hospitalNo)
    {
        await Task.Run(() =>
        {
            IEnumerable<XElement> op = from el in _doc.Descendants("hospital") where (string)el.Attribute("id") == hospitalNo select el;
            return op;
        });
        return null;
    }
}
