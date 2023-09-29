using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace surgical_reports.helpers
{


    public class reportMapper
    {
        private readonly IMapper _map;
        private IWebHostEnvironment _env;

        private IHospitalRepository _hos;

        private IEmployeeRepository _emp;

        private IUserRepository _user;

        public reportMapper(IMapper map, IWebHostEnvironment env, IUserRepository user, IHospitalRepository hos, IEmployeeRepository emp)
        {
            _map = map;
            _env = env;
            _user = user;
            _hos = hos;
            _emp = emp;
        }


        internal Class_Preview_Operative_report mapToClassPreviewOperativeReport(PreviewForReturnDTO pvfr, Class_Preview_Operative_report cp)
        {
            return _map.Map<PreviewForReturnDTO, Class_Preview_Operative_report>(pvfr, cp);
        }
        internal Class_privacy_model mapToClassPrivacyModel(PreviewForReturnDTO pvfr)
        {
            return _map.Map<PreviewForReturnDTO, Class_privacy_model>(pvfr);
        }

          public async Task<ReportHeaderDTO> mapToReportHeaderAsync(Class_Procedure proc)
        {
            var current_hospital = await _hos.GetSpecificHospital(proc.hospital.ToString());

            if (current_hospital == null) { return new ReportHeaderDTO(); }
            else
            {
                var current_user = await _user.GetUser(proc.SelectedSurgeon);
                var current_perfusionist = "";
                var current_anaesthesiologist = "";


                if (proc.SelectedPerfusionist == 0) { current_perfusionist = "n/a"; }
                else
                {
                    (await _emp.getSpecificEmployee(proc.SelectedPerfusionist)).name.UppercaseFirst();
                }
                if (proc.SelectedAnaesthesist == 0) { current_anaesthesiologist = "n/a"; }
                else
                {
                    (await _emp.getSpecificEmployee(proc.SelectedAnaesthesist)).name.UppercaseFirst();

                }

                var l = new List<string>();
                l = await this.getHeaderTextAsync(proc.hospital.ToString());

                var dto = new ReportHeaderDTO
                {
                    Id = proc.ProcedureId,
                    hospital_image = current_hospital.imageUrl,
                    hospital_city = l[0],
                    hospital_name = l[1],
                    hospital_number = l[2],
                    hospital_unit = l[3],
                    hospital_dept = l[4],
                    operation_date = proc.DateOfSurgery,

                    perfusionist = current_perfusionist,
                    surgeon = current_user.KnownAs.UppercaseFirst(),
                    physician = current_user.KnownAs.UppercaseFirst(),
                    anaesthesiologist = current_anaesthesiologist,
                    assistant = (await _user.GetUser(proc.SelectedAssistant))?.KnownAs.UppercaseFirst() ?? "n/a",
                    surgeon_picture = current_user.PhotoUrl,
                    diagnosis = "",
                    operation = proc.Description,
                    title = "Operative Report",
                    Comment_1 = proc.Comment1,
                    Comment_2 = proc.Comment2,
                    Comment_3 = proc.Comment3,
                };

                return dto;
            }
        }
      

            public async Task<Class_Final_operative_report> updateFinalReportAsync(Class_privacy_model pm, int procedure_id)
        {

            var help = new Class_Final_operative_report();
            help.procedure_id = procedure_id;

            Class_Procedure cp = await _context.Procedures.Include(a => a.ValvesUsed).FirstOrDefaultAsync(x => x.ProcedureId == help.procedure_id);

            // this is used to compile the final report from different sources

            ReportHeaderDTO currentHeader = await mapToReportHeaderAsync(cp);

            Class_Preview_Operative_report prev = await _context.Previews.FirstOrDefaultAsync(x => x.procedure_id == help.procedure_id);
           
            var loggedinUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == cp.SelectedSurgeon);


            var report_code = Convert.ToInt32(this.getReportCode(cp.fdType));
            if (report_code == 1)
            {
                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;



                Class_CABG cb = await getCabgDetailsAsync(procedure_id);


                help.Regel25 = translateCabgStuff(1, cb.B1_SITE);
                help.Regel26 = translateCabgStuff(2, cb.Q01);
                help.Regel27 = translateCabgStuff(4, cb.ANGLE01);
                help.Regel28 = translateCabgStuff(3, cb.DIAM01);

                help.Regel29 = translateCabgStuff(1, cb.B2_SITE);
                help.Regel30 = translateCabgStuff(2, cb.Q02);
                help.Regel31 = translateCabgStuff(4, cb.ANGLE02);
                help.Regel32 = translateCabgStuff(3, cb.DIAM02);

                help.Regel33 = translateCabgStuff(1, cb.B3_SITE);
                help.Regel34 = translateCabgStuff(2, cb.Q03);
                help.Regel35 = translateCabgStuff(4, cb.ANGLE03);
                help.Regel36 = translateCabgStuff(3, cb.DIAM03);

                help.Regel37 = translateCabgStuff(1, cb.B4_SITE);
                help.Regel38 = translateCabgStuff(2, cb.Q04);
                help.Regel39 = translateCabgStuff(4, cb.ANGLE04);
                help.Regel40 = translateCabgStuff(3, cb.DIAM04);

                help.Regel41 = translateCabgStuff(1, cb.B5_SITE);
                help.Regel42 = translateCabgStuff(2, cb.Q05);
                help.Regel43 = translateCabgStuff(4, cb.ANGLE05);
                help.Regel44 = translateCabgStuff(3, cb.DIAM05);

                help.Regel1 = translateCabgStuff(1, cb.B6_SITE);
                help.Regel2 = translateCabgStuff(2, cb.Q06);
                help.Regel3 = translateCabgStuff(4, cb.ANGLE06);
                help.Regel4 = translateCabgStuff(3, cb.DIAM06);

                help.Regel45 = "The course of the graft(s) is: " + cb.course;

                help = this.getGeneralCABGDetails(help, prev);

            }
            if (report_code == 2)
            {

                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;



                Class_CABG cb = await getCabgDetailsAsync(procedure_id);
                help.Regel25 = translateCabgStuff(1, cb.B1_SITE);
                help.Regel26 = translateCabgStuff(2, cb.Q01);
                help.Regel27 = translateCabgStuff(4, cb.ANGLE01);
                help.Regel28 = translateCabgStuff(3, cb.DIAM01);

                help.Regel29 = translateCabgStuff(1, cb.B2_SITE);
                help.Regel30 = translateCabgStuff(2, cb.Q02);
                help.Regel31 = translateCabgStuff(4, cb.ANGLE02);
                help.Regel32 = translateCabgStuff(3, cb.DIAM02);

                help.Regel33 = translateCabgStuff(1, cb.B3_SITE);
                help.Regel34 = translateCabgStuff(2, cb.Q03);
                help.Regel35 = translateCabgStuff(4, cb.ANGLE03);
                help.Regel36 = translateCabgStuff(3, cb.DIAM03);

                help.Regel37 = translateCabgStuff(1, cb.B4_SITE);
                help.Regel38 = translateCabgStuff(2, cb.Q04);
                help.Regel39 = translateCabgStuff(4, cb.ANGLE04);
                help.Regel40 = translateCabgStuff(3, cb.DIAM04);

                help.Regel41 = translateCabgStuff(1, cb.B5_SITE);
                help.Regel42 = translateCabgStuff(2, cb.Q05);
                help.Regel43 = translateCabgStuff(4, cb.ANGLE05);
                help.Regel44 = translateCabgStuff(3, cb.DIAM05);

                help.Regel1 = translateCabgStuff(1, cb.B6_SITE);
                help.Regel2 = translateCabgStuff(2, cb.Q06);
                help.Regel3 = translateCabgStuff(4, cb.ANGLE06);
                help.Regel4 = translateCabgStuff(3, cb.DIAM06);

                help.Regel45 = "The course of the graft(s) is: " + cb.course;

                help = this.getGeneralCABGDetails(help, prev);


            }
            if (report_code == 3)
            {
                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;

                // get the valve where implant position is 'Aortic'
            Class_Valve cv = await getValvesDetailsAsync("Aortic",cp.ProcedureId);


                if (cp.fdType == 3)
                { // aortic valve replacement
                    help.Regel25 = cv.MODEL;
                    help.Regel26 = cv.SIZE;
                    help.Regel27 = cv.SERIAL_IMP;
                }
                if (cp.fdType == 30)
                {// aortic valve replacement, minimally invasive approach
                    help.Regel25 = cv.MODEL;
                    help.Regel26 = cv.SIZE;
                    help.Regel27 = cv.SERIAL_IMP;
                }
                help = this.getGeneralDetails(help, prev);


            }
            if (report_code == 4)
            {
                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;

                Class_Valve cv = await getValvesDetailsAsync("Mitral",cp.ProcedureId);


                if (cp.fdType == 4)
                {// mitral valve replacement
                    help.Regel28 = cv.MODEL;
                    help.Regel29 = cv.SIZE;
                    help.Regel30 = cv.SERIAL_IMP;
                }
                if (cp.fdType == 41)
                {// mitral valve repair
                    help.Regel28 = cv.MODEL;
                    help.Regel29 = cv.SIZE;
                    help.Regel30 = cv.SERIAL_IMP;
                }
                help = this.getGeneralDetails(help, prev);

            }
            if (report_code == 5)
            {// double valve replacement

                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;

                // this will go in avr/mvr blok 2
                help.Regel31 = prev.regel_27;
                help.Regel32 = prev.regel_28;
                help.Regel33 = prev.regel_29;
                help.Regel34 = prev.regel_30;
                help.Regel35 = prev.regel_31;
                help.Regel36 = prev.regel_32;
                help.Regel37 = prev.regel_33;

                Class_Valve cv = await getValvesDetailsAsync("Aortic",cp.ProcedureId);

                help.Regel25 = cv.MODEL;
                help.Regel26 = cv.SIZE;
                help.Regel27 = cv.SERIAL_IMP;


                Class_Valve cvm = await getValvesDetailsAsync("Mitral",cp.ProcedureId);

                help.Regel28 = cvm.MODEL;
                help.Regel29 = cvm.SIZE;
                help.Regel30 = cvm.SERIAL_IMP;

                // this will go in avr/mvr blok 3
                help.Regel38 = prev.regel_15;
                help.Regel39 = prev.regel_16;
                help.Regel40 = prev.regel_17;
                help.Regel41 = prev.regel_18;
                help.Regel42 = prev.regel_19;
                help.Regel43 = prev.regel_20;
                help.Regel44 = prev.regel_21;
                help.Regel45 = prev.regel_22;
                help.Regel46 = prev.regel_23;




            }
            if (report_code == 6)
            {
                help.Regel17 = prev.regel_1;
                help.Regel18 = prev.regel_2;
                help.Regel19 = prev.regel_3;
                help.Regel20 = prev.regel_4;

                help.Regel21 = prev.regel_5;
                help.Regel22 = prev.regel_6;
                help.Regel23 = prev.regel_7;
                help.Regel24 = prev.regel_8;

                help.Regel25 = prev.regel_9;
                help.Regel26 = prev.regel_10;
                help.Regel27 = prev.regel_11;
                help.Regel28 = prev.regel_12;

                help.Regel29 = prev.regel_13;
                help.Regel30 = prev.regel_14;
                help.Regel31 = prev.regel_15;
                help.Regel32 = prev.regel_16;

                help.Regel33 = prev.regel_17;

                help.Regel34 = prev.regel_18;
                help.Regel35 = prev.regel_19;
                help.Regel36 = prev.regel_20;

                help.Regel37 = prev.regel_21;
                help.Regel38 = prev.regel_22;
                help.Regel39 = prev.regel_23;

                /*  help.Regel40 = prev.regel_1;

                 help.Regel41 = prev.regel_1;
                 help.Regel42 = prev.regel_1;
                 help.Regel43 = prev.regel_1;
                 help.Regel44 = prev.regel_1;  */

                help = this.getGeneralDetails(help, prev);


            }


            help.HospitalUrl = currentHeader.surgeon_picture;
            help.HeaderText1 = currentHeader.hospital_name;
            help.HeaderText2 = currentHeader.hospital_unit;
            help.HeaderText3 = currentHeader.hospital_dept;
            help.HeaderText4 = currentHeader.hospital_city;
            help.HeaderText5 = "Hospital number: " + pm.MedicalRecordNumber;
            help.HeaderText6 = "National ID: ";
            help.HeaderText7 = "Patient name: " + pm.patientName;
            help.HeaderText8 = "Physician: " + currentHeader.surgeon.UppercaseFirst();
            help.HeaderText9 = "";

            help.Regel10 = pm.Diagnosis.UppercaseFirst();
            help.Regel11 = currentHeader.operation;
            help.Regel12 = currentHeader.operation_date.ToString();

            help.Regel13 = currentHeader.surgeon;
            help.Regel14 = currentHeader.assistant;
            help.Regel15 = currentHeader.anaesthesiologist;
            help.Regel16 = currentHeader.perfusionist;

            help.Comment1 = cp.Comment1;
            help.Comment2 = cp.Comment2;
            help.Comment3 = cp.Comment3;

            help.UserName = loggedinUser.UserName;

            /*   help.AorticLineA = "";
              help.AorticLineB = "";
              help.AorticLineC = "";

              help.MitralLineA = "";
              help.MitralLineB = "";
              help.MitralLineC = ""; */



            return help;
        }



        private async Task<List<string>> getHeaderTextAsync(string current_hospital_id)
        {
            var help = new List<string>();
            var sh = await _hos.GetSpecificHospital(current_hospital_id.makeSureTwoChar());
          
            if (sh != null) // check if this id is in the list of hospitals
            {
                help.Add(sh.OpReportDetails1);
                help.Add(sh.OpReportDetails2);
                help.Add("Hospital No:");
                help.Add(sh.OpReportDetails4);
                help.Add(sh.OpReportDetails5);
            }
            return help;
        }
        private async Task<Class_CABG> getCabgDetailsAsync(int procedure_id)
        {
            var help = await _context.CABGS.FirstOrDefaultAsync(x => x.PROCEDURE_ID == procedure_id);
            return help;
        }
        private async Task<Class_Valve> getValvesDetailsAsync(string implantPosition, int procedure_id)
        {
            var help = await _context.Valves.FirstOrDefaultAsync(x => x.SERIAL_IMP == serial);
            return help;
        }
        public string getReportCode(int fdType)
        {
            var result = "";
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/procedure.xml");
            XDocument order = XDocument.Load(filename);
            IEnumerable<XElement> help = from d in order.Descendants("Code")
                                         where d.Element("ID").Value == fdType.ToString()
                                         select d;
            foreach (XElement x in help) { result = x.Element("report_code").Value; }
            return result;
        }
        private string translateCabgStuff(int soort, string test)
        {
            var result = "";
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/language_file.xml");
            XDocument order = XDocument.Load(filename);
            IEnumerable<XElement> help = from d in order.Descendants("cabg") select d;
            foreach (XElement x in help)
            {

                switch (soort)
                {
                    // locatie
                    case 1:
                        IEnumerable<XElement> rm = from d in order.Descendants("locatie").Elements("items") select d;
                        foreach (XElement el in rm)
                        {
                            if (el.Element("value").Value == test)
                            {
                                result = el.Element("description").Value;
                            }
                        }
                        break;
                    // quality
                    case 2:
                        IEnumerable<XElement> rm1 = from d in order.Descendants("quality").Elements("items") select d;
                        foreach (XElement el in rm1)
                        {
                            if (el.Element("value").Value == test)
                            {
                                result = el.Element("description").Value;
                            }
                        }
                        break;
                    // diameter
                    case 3:
                        IEnumerable<XElement> rm2 = from d in order.Descendants("diameter").Elements("items") select d;
                        foreach (XElement el in rm2)
                        {
                            if (el.Element("value").Value == test)
                            {
                                result = el.Element("description").Value;
                            }
                        }
                        break;
                    // angle
                    case 4:
                        IEnumerable<XElement> rm3 = from d in order.Descendants("angle").Elements("items") select d;
                        foreach (XElement el in rm3)
                        {
                            if (el.Element("value").Value == test)
                            {
                                result = el.Element("description").Value;
                            }
                        }
                        break;


                }





            }
            return result;
        }
        private Class_Final_operative_report getGeneralDetails(Class_Final_operative_report help, Class_Preview_Operative_report prev)
        {

            help.Regel46 = prev.regel_15;
            help.Regel47 = prev.regel_16;
            help.Regel48 = prev.regel_17;
            help.Regel49 = prev.regel_18;
            help.Regel50 = prev.regel_19;

            help.Regel51 = prev.regel_20;
            help.Regel52 = prev.regel_21;
            help.Regel53 = prev.regel_22;
            help.Regel54 = prev.regel_23;

            help.Regel58 = prev.regel_9;
            help.Regel59 = prev.regel_10;
            help.Regel60 = prev.regel_11;

            help.Regel61 = prev.regel_12;
            help.Regel62 = prev.regel_13;
            help.Regel63 = prev.regel_14;

            help.Regel31 = prev.regel_27;
            help.Regel32 = prev.regel_28;
            help.Regel33 = prev.regel_29;
            help.Regel34 = prev.regel_30;
            help.Regel35 = prev.regel_31;
            help.Regel36 = prev.regel_32;




            return help;

        }
        private Class_Final_operative_report getGeneralCABGDetails(Class_Final_operative_report help, Class_Preview_Operative_report prev)
        {
            help.Regel46 = prev.regel_15;
            help.Regel47 = prev.regel_16;
            help.Regel48 = prev.regel_17;
            help.Regel49 = prev.regel_18;
            help.Regel50 = prev.regel_19;

            help.Regel51 = prev.regel_20;
            help.Regel52 = prev.regel_21;
            help.Regel53 = prev.regel_22;
            help.Regel54 = prev.regel_23;

            help.Regel58 = prev.regel_9;
            help.Regel59 = prev.regel_10;
            help.Regel60 = prev.regel_11;

            help.Regel61 = prev.regel_12;
            help.Regel62 = prev.regel_13;
            help.Regel63 = prev.regel_14;
            return help;
        }
        public HospitalForReturnDTO mapToHospitalForReturn(Class_Hospital x) { return _map.Map<Class_Hospital, HospitalForReturnDTO>(x); }
        public Class_Hospital mapToHospital(HospitalForReturnDTO x, Class_Hospital h) { h = _map.Map<HospitalForReturnDTO, Class_Hospital>(x, h); return h; }
      
    }
}