namespace surgical_reports.implementations;

public class ComposeFinalReport : IComposeFinalReport
{
    private readonly DapperContext _context;

    public ComposeFinalReport(DapperContext context)
    {
        _context = context;
    }

    public int addToExpiredReports(ReportTiming rt)
    {
        throw new NotImplementedException();
    }
    public Task composeAsync(int procedure_id)
    {
        throw new NotImplementedException();
    }
    public async Task<Class_Final_operative_report> CreateFinalReport(frDto fr)
    {
        var query = "INSERT INTO finalReports (procedure_id, MedRecNumber, Name, ProcedureDescription, Attending, OperatieDate, Diagnosis, " +
        "Surgeon, Assistant, Perfusionist, Anaesthesist,FreeText, " +
        "HeaderText1,HeaderText2,HeaderText3,HeaderText4,HeaderText5,HeaderText6,HeaderText7,HeaderText8,HeaderText9, " +
        "Regel1, Regel2, Regel3, Regel4, Regel5, Regel6, Regel7, Regel8, Regel9, Regel10, " +
        "Regel11, Regel12, Regel13, Regel14, Regel15, Regel16, Regel17, Regel18, Regel19, Regel20, " +
        "Regel21, Regel22, Regel23, Regel24, Regel25, Regel26, Regel27, Regel28, Regel29, Regel30, " +
        "Regel31, Regel32, Regel33, Regel34, Regel35, Regel36, Regel37, Regel38, Regel39, Regel40, " +
        "Regel41, Regel42, Regel43, Regel44, Regel45, Regel46, Regel47, Regel48, Regel49, Regel50, " +
        "Regel51, Regel52, Regel53, Regel54, Regel55, Regel56, Regel57, Regel58, Regel59, Regel60, " +
        "Regel61, Regel62, Regel63, " +
        "Comment1, Comment2, Comment3, " +
        "UserName, HospitalUrl, " +
        "AorticLineA, AorticLineB, AorticLineC, " +
        "MitralLineA, MitralLineB, MitralLineC)" +
        " VALUES (@procedure_id, @MedRecNumber, @Name, @ProcedureDescription, @Attending, @OperatieDate, @Diagnosis, " +
        "@Surgeon, @Assistant, @Perfusionist, @Anaesthesist ,@FreeText, " +
        "@HeaderText1,@HeaderText2,@HeaderText3,@HeaderText4,@HeaderText5,@HeaderText6,@HeaderText7,@HeaderText8,@HeaderText9, " +
        "@Regel1, @Regel2, @Regel3, @Regel4, @Regel5, @Regel6, @Regel7, @Regel8, @Regel9, @Regel10, " +
        "@Regel11, @Regel12, @Regel13, @Regel14, @Regel15, @Regel16, @Regel17, @Regel18, @Regel19, @Regel20, " +
        "@Regel21, @Regel22, @Regel23, @Regel24, @Regel25, @Regel26, @Regel27, @Regel28, @Regel29, @Regel30, " +
        "@Regel31, @Regel32, @Regel33, @Regel34, @Regel35, @Regel36, @Regel37, @Regel38, @Regel39, @Regel40, " +
        "@Regel41, @Regel42, @Regel43, @Regel44, @Regel45, @Regel46, @Regel47, @Regel48, @Regel49, @Regel50, " +
        "@Regel51, @Regel52, @Regel53, @Regel54, @Regel55, @Regel56, @Regel57, @Regel58, @Regel59, @Regel60, " +
        "@Regel61, @Regel62, @Regel63, " +
        "@Comment1, @Comment2, @Comment3, " +
        "@UserName, @HospitalUrl, " +
        "@AorticLineA, @AorticLineB, @AorticLineC, " +
        "@MitralLineA, @MitralLineB, @MitralLineC);" + "SELECT LAST_INSERT_ID();";

        var parameters = new DynamicParameters();

        parameters.Add("procedure_id", fr.procedure_id, DbType.Int32);
        parameters.Add("MedRecNumber", fr.MedRecNumber, DbType.String);
        parameters.Add("Name", fr.Name, DbType.String);
        parameters.Add("ProcedureDescription", fr.ProcedureDescription, DbType.String);
        parameters.Add("Attending", fr.Attending, DbType.String);
        parameters.Add("OperatieDate", fr.OperatieDate, DbType.String);
        parameters.Add("Diagnosis", fr.Diagnosis, DbType.String);
        parameters.Add("Surgeon", fr.Surgeon, DbType.String);
        parameters.Add("Assistant", fr.Assistant, DbType.String);
        parameters.Add("Perfusionist", fr.Perfusionist, DbType.String);
        parameters.Add("Anaesthesist", fr.Anaesthesist, DbType.String);
        parameters.Add("FreeText", fr.FreeText, DbType.String);

        parameters.Add("HeaderText1", fr.HeaderText1, DbType.String);
        parameters.Add("HeaderText2", fr.HeaderText2, DbType.String);
        parameters.Add("HeaderText3", fr.HeaderText3, DbType.String);
        parameters.Add("HeaderText4", fr.HeaderText4, DbType.String);
        parameters.Add("HeaderText5", fr.HeaderText5, DbType.String);
        parameters.Add("HeaderText6", fr.HeaderText6, DbType.String);
        parameters.Add("HeaderText7", fr.HeaderText7, DbType.String);
        parameters.Add("HeaderText8", fr.HeaderText8, DbType.String);
        parameters.Add("HeaderText9", fr.HeaderText9, DbType.String);

        parameters.Add("Regel1", fr.Regel1, DbType.String);
        parameters.Add("Regel2", fr.Regel2, DbType.String);
        parameters.Add("Regel3", fr.Regel3, DbType.String);
        parameters.Add("Regel4", fr.Regel4, DbType.String);
        parameters.Add("Regel5", fr.Regel5, DbType.String);
        parameters.Add("Regel6", fr.Regel6, DbType.String);
        parameters.Add("Regel7", fr.Regel7, DbType.String);
        parameters.Add("Regel8", fr.Regel8, DbType.String);
        parameters.Add("Regel9", fr.Regel9, DbType.String);
        parameters.Add("Regel10", fr.Regel10, DbType.String);
        parameters.Add("Regel11", fr.Regel11, DbType.String);
        parameters.Add("Regel12", fr.Regel12, DbType.String);
        parameters.Add("Regel13", fr.Regel13, DbType.String);
        parameters.Add("Regel14", fr.Regel14, DbType.String);
        parameters.Add("Regel15", fr.Regel15, DbType.String);
        parameters.Add("Regel16", fr.Regel16, DbType.String);
        parameters.Add("Regel17", fr.Regel17, DbType.String);
        parameters.Add("Regel18", fr.Regel18, DbType.String);
        parameters.Add("Regel19", fr.Regel19, DbType.String);
        parameters.Add("Regel20", fr.Regel20, DbType.String);
        parameters.Add("Regel21", fr.Regel21, DbType.String);
        parameters.Add("Regel22", fr.Regel22, DbType.String);
        parameters.Add("Regel23", fr.Regel23, DbType.String);
        parameters.Add("Regel24", fr.Regel24, DbType.String);
        parameters.Add("Regel25", fr.Regel25, DbType.String);
        parameters.Add("Regel26", fr.Regel26, DbType.String);
        parameters.Add("Regel27", fr.Regel27, DbType.String);
        parameters.Add("Regel28", fr.Regel28, DbType.String);
        parameters.Add("Regel29", fr.Regel29, DbType.String);
        parameters.Add("Regel30", fr.Regel30, DbType.String);
        parameters.Add("Regel31", fr.Regel31, DbType.String);
        parameters.Add("Regel32", fr.Regel32, DbType.String);
        parameters.Add("Regel33", fr.Regel33, DbType.String);
        parameters.Add("Regel34", fr.Regel34, DbType.String);
        parameters.Add("Regel35", fr.Regel35, DbType.String);
        parameters.Add("Regel36", fr.Regel36, DbType.String);
        parameters.Add("Regel37", fr.Regel37, DbType.String);
        parameters.Add("Regel38", fr.Regel38, DbType.String);
        parameters.Add("Regel39", fr.Regel38, DbType.String);
        parameters.Add("Regel40", fr.Regel40, DbType.String);
        parameters.Add("Regel41", fr.Regel41, DbType.String);
        parameters.Add("Regel42", fr.Regel42, DbType.String);
        parameters.Add("Regel43", fr.Regel43, DbType.String);
        parameters.Add("Regel44", fr.Regel44, DbType.String);
        parameters.Add("Regel45", fr.Regel45, DbType.String);
        parameters.Add("Regel46", fr.Regel46, DbType.String);
        parameters.Add("Regel47", fr.Regel47, DbType.String);
        parameters.Add("Regel48", fr.Regel48, DbType.String);
        parameters.Add("Regel49", fr.Regel49, DbType.String);
        parameters.Add("Regel50", fr.Regel50, DbType.String);
        parameters.Add("Regel51", fr.Regel51, DbType.String);
        parameters.Add("Regel52", fr.Regel52, DbType.String);
        parameters.Add("Regel53", fr.Regel53, DbType.String);
        parameters.Add("Regel54", fr.Regel54, DbType.String);
        parameters.Add("Regel55", fr.Regel55, DbType.String);
        parameters.Add("Regel56", fr.Regel56, DbType.String);
        parameters.Add("Regel57", fr.Regel57, DbType.String);
        parameters.Add("Regel58", fr.Regel58, DbType.String);
        parameters.Add("Regel59", fr.Regel58, DbType.String);
        parameters.Add("Regel60", fr.Regel60, DbType.String);
        parameters.Add("Regel61", fr.Regel61, DbType.String);
        parameters.Add("Regel62", fr.Regel62, DbType.String);
        parameters.Add("Regel63", fr.Regel63, DbType.String);

        parameters.Add("Comment1", fr.Comment1, DbType.String);
        parameters.Add("Comment2", fr.Comment2, DbType.String);
        parameters.Add("Comment3", fr.Comment3, DbType.String);

        parameters.Add("UserName", fr.UserName, DbType.String);
        parameters.Add("HospitalUrl", fr.HospitalUrl, DbType.String);
        parameters.Add("AorticLineA", fr.AorticLineA, DbType.String);
        parameters.Add("AorticLineB", fr.AorticLineB, DbType.String);
        parameters.Add("AorticLineC", fr.AorticLineC, DbType.String);
        parameters.Add("MitralLineA", fr.MitralLineA, DbType.String);
        parameters.Add("MitralLineB", fr.MitralLineB, DbType.String);
        parameters.Add("MitralLineC", fr.MitralLineC, DbType.String);



        using (var connection = _context.CreateConnection())
        {
            var id = await connection.QuerySingleAsync<int>(query, parameters);
            var createdFinalReport = new Class_Final_operative_report
            {

                Id = id,
                procedure_id = fr.procedure_id,
                MedRecNumber = fr.MedRecNumber,
                Name = fr.Name,
                ProcedureDescription = fr.ProcedureDescription,
                Attending = fr.Attending,
                OperatieDate = fr.OperatieDate,
                Diagnosis = fr.Diagnosis,
                Surgeon = fr.Surgeon,
                Assistant = fr.Assistant,
                Perfusionist = fr.Perfusionist,
                Anaesthesist = fr.Anaesthesist,
                FreeText = fr.FreeText,
                HeaderText1 = fr.HeaderText1,
                HeaderText2 = fr.HeaderText2,
                HeaderText3 = fr.HeaderText3,
                HeaderText4 = fr.HeaderText4,
                HeaderText5 = fr.HeaderText5,
                HeaderText6 = fr.HeaderText6,
                HeaderText7 = fr.HeaderText7,
                HeaderText8 = fr.HeaderText8,
                HeaderText9 = fr.HeaderText9,
                Regel1 = fr.Regel1,
                Regel2 = fr.Regel2,
                Regel3 = fr.Regel3,
                Regel4 = fr.Regel4,
                Regel5 = fr.Regel5,
                Regel6 = fr.Regel6,
                Regel7 = fr.Regel7,
                Regel8 = fr.Regel8,
                Regel9 = fr.Regel9,
                Regel10 = fr.Regel10,

                Regel11 = fr.Regel11,
                Regel12 = fr.Regel12,
                Regel13 = fr.Regel13,
                Regel14 = fr.Regel14,
                Regel15 = fr.Regel15,
                Regel16 = fr.Regel16,
                Regel17 = fr.Regel17,
                Regel18 = fr.Regel18,
                Regel19 = fr.Regel19,
                Regel20 = fr.Regel20,

                Regel21 = fr.Regel21,
                Regel22 = fr.Regel22,
                Regel23 = fr.Regel23,
                Regel24 = fr.Regel24,
                Regel25 = fr.Regel25,
                Regel26 = fr.Regel26,
                Regel27 = fr.Regel27,
                Regel28 = fr.Regel28,
                Regel29 = fr.Regel29,
                Regel30 = fr.Regel30,

                Regel31 = fr.Regel31,
                Regel32 = fr.Regel32,
                Regel33 = fr.Regel33,
                Regel34 = fr.Regel34,
                Regel35 = fr.Regel35,
                Regel36 = fr.Regel36,
                Regel37 = fr.Regel37,
                Regel38 = fr.Regel38,
                Regel39 = fr.Regel39,
                Regel40 = fr.Regel40,

                Regel41 = fr.Regel41,
                Regel42 = fr.Regel42,
                Regel43 = fr.Regel43,
                Regel44 = fr.Regel44,
                Regel45 = fr.Regel45,
                Regel46 = fr.Regel46,
                Regel47 = fr.Regel47,
                Regel48 = fr.Regel48,
                Regel49 = fr.Regel49,
                Regel50 = fr.Regel50,

                Regel51 = fr.Regel51,
                Regel52 = fr.Regel52,
                Regel53 = fr.Regel53,
                Regel54 = fr.Regel54,
                Regel55 = fr.Regel55,
                Regel56 = fr.Regel56,
                Regel57 = fr.Regel57,
                Regel58 = fr.Regel58,
                Regel59 = fr.Regel59,
                Regel60 = fr.Regel60,

                Regel61 = fr.Regel61,
                Regel62 = fr.Regel62,
                Regel63 = fr.Regel63,

                Comment1 = fr.Comment1,
                Comment2 = fr.Comment2,
                Comment3 = fr.Comment3,
                UserName = fr.UserName,
                HospitalUrl = fr.HospitalUrl,
                AorticLineA = fr.AorticLineA,
                AorticLineB = fr.AorticLineB,
                AorticLineC = fr.AorticLineC,
                MitralLineA = fr.MitralLineA,
                MitralLineB = fr.MitralLineB,
                MitralLineC = fr.MitralLineC
            };
            return createdFinalReport;
        }
    }
    public int deleteExpiredReports()
    {
        throw new NotImplementedException();
    }
    public int deletePDF(int id)
    {
        throw new NotImplementedException();
    }
    public async Task<List<Class_Final_operative_report>> getFinalReports()
    {
        var query = "SELECT * FROM finalReports";
        using (var connection = _context.CreateConnection())
        {
            var reports = await connection.QueryAsync<Class_Final_operative_report>(query);
            return reports.ToList();
        }
    }
    public Task<int> getReportCode(int procedure_id)
    {
        throw new NotImplementedException();
    }
    public async Task<Class_Final_operative_report> getSpecificReport(int id)
    {
        var query = "SELECT * FROM finalReports WHERE procedure_id = @id";
        using (var connection = _context.CreateConnection())
        {
            var report = await connection.QuerySingleOrDefaultAsync<Class_Final_operative_report>(query, new {id});
            return report;
        }
    }
    public Task<bool> isReportExpired(int id)
    {
        throw new NotImplementedException();
    }
}
