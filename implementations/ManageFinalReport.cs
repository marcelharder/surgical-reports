
namespace surgical_reports.implementations;

public class ManageFinalReport : IManageFinalReport
{
    private readonly IWebHostEnvironment _env;


    public ManageFinalReport(IWebHostEnvironment env)
    {
        _env = env;

    }
    public int DeletePDF(int id)
    {
        var idString = id.ToString();
        var pathToFile = Path.Combine(_env.ContentRootPath, "assets", "pdf");
        var fileName = Path.Combine(pathToFile, $"{idString}.pdf");

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
            Thread.Sleep(20);
        }
        return 1;

    }
    public int AddToExpiredReports(ReportTiming rt)
    {
        var l = GetXmlDetails();
        bool changesMade = false;

        if (!l.Any(r => r.id == rt.id))
        {
            l.Add(rt);
            changesMade = true;
        }

        if (changesMade)
        {
            SaveXmlDetails(l);
        }

        return 3;
    }
    public async Task<bool> IsReportExpired(int id)
    {
        var result = false;
        // find out if this report is expired
        await Task.Run(() =>
        {
            var l = new List<ReportTiming>();
            l = GetXmlDetails();  // load the xml file im a list of ReportTimings
            foreach (ReportTiming rep in l)
            {
                if (rep.id == id)
                {
                    var currentTicks = DateTime.Now.Ticks;
                    var interval = TimeSpan.FromDays(3).Ticks;
                    var publishTime = new DateTime(rep.publishTime.Ticks);
                    var expirationTime = publishTime.Add(TimeSpan.FromTicks(interval));

                    if (expirationTime.Ticks < currentTicks)
                    {
                        // report is expired
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                }
            }
        });
        return result;
    }
    public int DeleteExpiredReports()
    {
        var currentTicks = DateTime.UtcNow.Ticks; // use UTC time instead of local time
        var interval = TimeSpan.FromDays(3).Ticks; // use TimeSpan to define interval

        try
        {
            // load the xml file im a list of ReportTimings
            var reportTimings = new List<ReportTiming>();
            var pathToFile = Path.Combine(_env.ContentRootPath, "xml", "timingsRefReport.xml");
            if (!File.Exists(pathToFile)) { return 2; }
            else
            {
                try
                {
                    using (var stream = File.Open(pathToFile, FileMode.Open))
                    {
                        var serializer = new XmlSerializer(typeof(List<ReportTiming>));
                        reportTimings = (List<ReportTiming>)serializer.Deserialize(stream);
                    }
                }
                catch (Exception e) { // the root element is mising
                                    Console.Write(e.InnerException); }
            }
            // filter on expired report timings
            var expiredReportTimings = reportTimings.Where(rt => (rt.publishTime.Ticks + interval) < currentTicks).ToList();
            foreach (ReportTiming rt in expiredReportTimings) { DeletePDF(rt.id); } // delete expired pdf's

            // filter out expired report timings
            var newReportTimings = reportTimings.Where(rt => (rt.publishTime.Ticks + interval) >= currentTicks).ToList();

            // write new report timings to file
            SaveXmlDetails(newReportTimings);

            return 1;
        }
        catch (Exception ex)
        {
            // log the exception instead of returning ret value
            Console.WriteLine("Failed to delete expired reports: " + ex.Message);
            return 2;
        }
    }
    
    public List<ReportTiming> GetXmlDetails()
    {
        // load the xml file into a list of ReportTimings
        var pathToFile = Path.Combine(_env.ContentRootPath, "xml", "timingsRefReport.xml");

        if (!File.Exists(pathToFile))
        {
            return new List<ReportTiming>();
        }

        using (var stream = File.Open(pathToFile, FileMode.Open))
        {
            var serializer = new XmlSerializer(typeof(List<ReportTiming>));
            return (List<ReportTiming>)serializer.Deserialize(stream);
        }




    }
    public void SaveXmlDetails(List<ReportTiming> reportTimings)
    {
        // save the list of ReportTimings to an xml file
        var pathToFile = Path.Combine(_env.ContentRootPath, "xml", "timingsRefReport.xml");

        using (var stream = File.Open(pathToFile, FileMode.Create))
        {
            var serializer = new XmlSerializer(typeof(List<ReportTiming>));
            serializer.Serialize(stream, reportTimings);
        }
    }
    public async Task<bool> PdfDoesNotExists(string id_string)
    {
        var pathToFile = Path.Combine(_env.ContentRootPath, "assets", "pdf", $"{id_string}.pdf");
        var fileExists = await Task.Run(() => File.Exists(pathToFile));
        return !fileExists;
    }


}

