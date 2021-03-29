<<<<<<< HEAD
﻿using OpenQA.Selenium;
=======
﻿using MVPStudio.Framework.Helps.Excel;
>>>>>>> Damart/qa.onboarding.specflow-TC-443-2
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Talent.Automation.Model
{
    public class JobPost
    {
        public const decimal MaximumSalaryCap = 150000m;
        public string Company { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string YearsOfExperience { get; set; }

        private string qualificationTypes;
        public string QualificationTypes
        {
            get { return qualificationTypes;  }
            set
            {
                qualificationTypes = value;
                QualificationTypeList.Clear();
                if (value == null)
                {
                    return;
                }
                foreach (string qualificationType in qualificationTypes.Split(','))
                {
                    QualificationTypeList.Add(qualificationType.Trim());
                }
            }
        }
        public IList<string> QualificationTypeList { get; } = new List<string>();

        private string visaStatusTypes;
        public string VisaStatusTypes
        {
            get { return visaStatusTypes; }
            set
            {
                visaStatusTypes = value;
                VisaStatusTypeList.Clear();
                if (value == null)
                {
                    return;
                }
                foreach (string visaStatusType in visaStatusTypes.Split(','))
                {
                    VisaStatusTypeList.Add(visaStatusType.Trim());
                }
            }
        }
        public IList<string> VisaStatusTypeList { get; } = new List<string>();

        private string jobTypes;
        public string JobTypes
        {
            get { return jobTypes; }
            private set
            {
                jobTypes = value;
                JobTypeList.Clear();
                if (value == null)
                {
                    return;
                }
                foreach (string jobType in jobTypes.Split(','))
                {
                    JobTypeList.Add(jobType.Trim());
                }
            }
        }
        public IList<string> JobTypeList { get; } = new List<string>();
        private int minimumSalary;
        public int MinimumSalary
        {
            get
            {
                return minimumSalary;
            }
            set
            {
                minimumSalary = value;
                if (minimumSalary < 0)
                {
                    minimumSalary = 0;
                }
                else if (minimumSalary > MaximumSalaryCap)
                {
                    minimumSalary = decimal.ToInt32(MaximumSalaryCap);
                }
            } 
        }
        private int maximumSalary;
        public int MaximumSalary
        {
            get
            {
                return maximumSalary;
            }
            set
            {
                maximumSalary = value;
                if (maximumSalary < 0)
                {
                    maximumSalary = 0;
                }
                else if (maximumSalary > MaximumSalaryCap)
                {
                    maximumSalary = decimal.ToInt32(MaximumSalaryCap);
                }
            }
        }
        public string Country { get; set; }
        public string City { get; set; }
        public string StartDate
        {
            get
            {
                if (DateTime.TryParseExact(_startDate?.ToString("", CultureInfo.InvariantCulture) ?? _startDateString,
                                       "yyyy-MM-dd",
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None,
                                       out DateTime startDate))
                {
                    return startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                return _startDateString;
            }
            set
            {
                _startDateString = value;
                _startDate = null;
                if (!string.IsNullOrEmpty(_startDateString))
                {
                    _startDate = DateTime.TryParse(_startDateString, out DateTime output) ? output : (DateTime?)null;
                }
            }
        }
        private string _startDateString;

        private DateTime? _startDate = DateTime.Today; // default is today

        private DateTime? _endDate;
        public string EndDate
        {
            get
            {
                if (DateTime.TryParseExact(_endDate?.ToString(CultureInfo.InvariantCulture) ?? _endDateString,
                                       "yyyy-MM-dd",
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None,
                                       out DateTime endate))
                {
                    return endate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                return _endDateString;
            }
            set
            {
                _endDateString = value;
                _endDate = null;
                if (!string.IsNullOrEmpty(_endDateString))
                {
                    _endDate = DateTime.TryParse(_endDateString, out DateTime output) ? output : (DateTime?)null;
                }
            }
        }
        private string _endDateString;

        private DateTime? _expiryDate = DateTime.Today.AddDays(14); // 14 days
        public string ExpiryDate
        {
            get
            {
                if (DateTime.TryParseExact(_expiryDate?.ToString(CultureInfo.InvariantCulture)??_expiryDateString,
                                       "yyyy-MM-dd",
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None,
                                       out DateTime expiryDate))
                {
                    return expiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                return _expiryDateString;
            }
            set
            {
                _expiryDateString = value;
                _expiryDate = null;
                if (!string.IsNullOrEmpty(_expiryDateString))
                {
                    _expiryDate = DateTime.TryParse(_expiryDateString, out DateTime output) ? output : (DateTime?)null;
                }
            }
        }
        private string _expiryDateString;
<<<<<<< HEAD
        private IWebDriver driver;

        public JobPost(IWebDriver driver)
        {
            this.driver = driver;
        }
=======

        public static JobPost CreateNewJob(String key)
        {
            ExcelUtil.SetDataSource("Job.xlsx");
            ExcelData jobDetails = ExcelUtil.DataSet.SelectSheet("ADD").GetRowByKey(key);
            JobPost NewJob = new JobPost();
            NewJob.Company = jobDetails.GetValue("Company");
            NewJob.Title = jobDetails.GetValue("Title");
            NewJob.Summary = jobDetails.GetValue("Summary") + " " + DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            NewJob.Description = jobDetails.GetValue("Description");
            NewJob.Category = jobDetails.GetValue("Category");
            NewJob.SubCategory = jobDetails.GetValue("SubCategory");
            NewJob.YearsOfExperience = jobDetails.GetValue("YearsOfExperience");
            NewJob.QualificationTypes = jobDetails.GetValue("QualificationTypes");
            NewJob.VisaStatusTypes = jobDetails.GetValue("VisaStatusTypes");
            NewJob.JobTypes = jobDetails.GetValue("JobTypes");
            NewJob.MinimumSalary = int.Parse(jobDetails.GetValue("MinimumSalary"));
            NewJob.MaximumSalary = int.Parse(jobDetails.GetValue("MaximumSalary"));
            NewJob.Country = jobDetails.GetValue("Country");
            NewJob.City = jobDetails.GetValue("City");
            NewJob.EndDate = jobDetails.GetValue("EndDate");
            return NewJob;
        }

>>>>>>> Damart/qa.onboarding.specflow-TC-443-2
    }
}
