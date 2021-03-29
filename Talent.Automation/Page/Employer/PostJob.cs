using Dynamitey.DynamicObjects;
using MVPStudio.Framework.Extensions;
using MVPStudio.Framework.Helps.Excel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Talent.Automation.Model;
using Talent.Automation.Steps.BaseStep;
using TechTalk.SpecFlow;

namespace Talent.Automation.Page.Employer
{
    public class PostJob : Base
    {
        public PostJob(IWebDriver driver) : base(driver)
        {
            
        }

        private IWebElement Company => Driver.WaitForElement(By.Id("jobPost_company"));
        private IWebElement Title => Driver.WaitForElement(By.Id("jobPost_title"));
        private IWebElement Summary => Driver.WaitForElement(By.Id("jobPost_summary"));
        private IWebElement Description => Driver.WaitForElement(By.CssSelector("#jobPost_description > .ql-container.ql-bubble > .ql-editor"));
        private IWebElement Category => Driver.WaitForElement(By.CssSelector("#jobPost_category > div"));
        private IWebElement SubCategory => Driver.WaitForElement(By.CssSelector("#jobPost_subCategory > div"));
        private IWebElement YearsOfExperience => Driver.WaitForElement(By.Id("jobPost_experience"));
        private static By QualificationTypes => By.CssSelector("#jobPost_qualification > .ant-checkbox-group-item > .ant-checkbox input");
        private static By VisaStatusTypes => By.CssSelector("#jobPost_visaStatus  > .ant-checkbox-group-item > .ant-checkbox input");
        private static By JobTypes => By.CssSelector("#jobPost_jobType > .ant-checkbox-group-item > .ant-checkbox input");
        private readonly string _salaryRangeXPathPrefix = "//label[@for='jobPost_salaryRange']/parent::div/following-sibling::div/descendant::div";
        private IWebElement SalaryRangeSlider => Driver.WaitForElement(By.XPath($"{_salaryRangeXPathPrefix}[@class='ant-slider-rail']"));
        private IWebElement MaximumSalaryCap => Driver.WaitForClickable(By.XPath($"{_salaryRangeXPathPrefix}[contains(@class, 'ant-slider-handle-2')]"));
        private IWebElement MinimumSalaryCap => Driver.WaitForClickable(By.XPath($"{_salaryRangeXPathPrefix}[contains(@class, 'ant-slider-handle-1')]"));
        private IWebElement Country => Driver.WaitForElement(By.CssSelector("#jobPost_country > div"));
        private IWebElement City => Driver.WaitForElement(By.CssSelector("#jobPost_city > div"));
        private IWebElement StartDate => Driver.WaitForElement(By.CssSelector("input[placeholder='Start Date']")); // does not have id
        private IWebElement EndDate => Driver.WaitForElement(By.CssSelector("#jobPost_endDate input"));
        private IWebElement ExpiryDate => Driver.WaitForElement(By.CssSelector("#jobPost_expiryDate input"));
        private IWebElement Save => Driver.WaitForClickable(By.XPath("//button[.='Save']"));
        private IWebElement Cancel => Driver.WaitForClickable(By.XPath("//button[.='Cancel']"));

        private IWebElement MandatoryTitle => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[1]"));
        private IWebElement MandatorySummary => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[2]"));
        private IWebElement MandatoryDescription => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[3]"));
        private IWebElement MandatoryCategory => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[4]"));
        private IWebElement MandatorySubCategory => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[5]"));
        private IWebElement MandatoryExperience => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[6]"));
        private IWebElement MandatoryQualification => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[7]"));
        private IWebElement MandatoryVisaStatus => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[8]"));
        private IWebElement MandatoryJobType => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[9]"));
        private IWebElement MandatoryCountry => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[10]"));
        private IWebElement MandatoryCity => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[11]"));
        private IWebElement InvalidFormatDate => Driver.WaitForElement(By.XPath("//div[@class='ant-form-explain']"));
        private IWebElement MandatoryEndDate => Driver.WaitForElement(By.XPath("(//div[@class='ant-form-explain'])[13]"));

        private IWebElement JobTitle => Driver.WaitForElement(By.XPath("(//span)[14]"));
        private IWebElement Actualyear => Driver.WaitForElement(By.XPath("//input[@aria-valuenow]"));
        private IWebElement FirstJobInList => Driver.WaitForElement(By.CssSelector("div.ant-card-body"), 20);


        public string PostJobTitle()
        {
            return Driver.Title;
        }

        //Scenario-Post a job(Save/Cancel)
        public void EnterJob()
        {
            ExcelData row = ExcelUtil.DataSet.SelectSheet("ADD").GetRowByKey("Valid");
            Company.WaitForClickable(Driver);
            Company.SendKeys(row.GetValue("Company"));
            Title.SendKeys(row.GetValue("Title"));
            Summary.SendKeys(row.GetValue("Summary"));
            Description.SendKeys(row.GetValue("Description"));

            Category.SelectDropDownValue(Driver, row.GetValue("Category"));
            SubCategory.SelectDropDownValue(Driver, row.GetValue("SubCategory"));
            YearsOfExperience.EnterText(row.GetValue("YearsOfExperience"));

            CheckBoxGroups();

            //Actions actions = new Actions(Driver);
            //actions.ClickAndHold(SalaryRangeSlider);
            //actions.MoveByOffset(2, 2);
            //actions.Build().Perform();

            //Slider action
            MinimumSalaryCap.DragAndDrop(Driver, 100);
            MaximumSalaryCap.DragAndDrop(Driver, 160);

            Country.SelectDropDownValue(Driver, row.GetValue("Country"));
            City.SelectDropDownValue(Driver, row.GetValue("City"));

            // Date fields
            StartDate.SelectDate(Driver, row.GetValue("StartDate"));
            EndDate.SelectDate(Driver, row.GetValue("EndDate"));
            ExpiryDate.SelectDate(Driver, row.GetValue("ExpiryDate"));

            if (row.GetValue("Save/Cancel") == "Save")
            {
                Save.Click();
            }
            else if (row.GetValue("Save/Cancel") == "Cancel")
            {
                Cancel.Click();
            }

        }

        //CheckBox Group
        public void CheckBoxGroups()
        {
            ExcelData row = ExcelUtil.DataSet.SelectSheet("ADD").GetRowByKey("Valid");
            //Select qualification from checkbox
            IList<IWebElement> Qualification = Driver.FindElements(By.CssSelector("#jobPost_qualification > .ant-checkbox-group-item > .ant-checkbox input"));
            int iSize = Qualification.Count;
            //Driver.SelectFromCheckBoxGroup(QualificationTypes, (IList<string>)QualificationTypes);
            for (int i = 0; i < iSize; i++)
            {
                String Value = Qualification.ElementAt(i).GetAttribute("value");
                if (Value == row.GetValue("QualificationTypes"))
                {
                    Qualification.ElementAt(i).Click();
                    // This will take the execution out of for loop
                    break;
                }
            }

            //Select Visastatus from checkbox
            IList<IWebElement> VisaStatus = Driver.FindElements(By.CssSelector("#jobPost_visaStatus  > .ant-checkbox-group-item > .ant-checkbox input"));
            int iSiz = VisaStatus.Count;
            for (int i = 0; i < iSiz; i++)
            {
                String Value1 = VisaStatus.ElementAt(i).GetAttribute("value");
                if (Value1 == row.GetValue("VisaStatusTypes"))
                {
                    VisaStatus.ElementAt(i).Click();
                    // This will take the execution out of for loop
                    break;
                }
            }

            //Select JobTypes from checkbox
            IList<IWebElement> JobTypes = Driver.FindElements(By.CssSelector("#jobPost_jobType > .ant-checkbox-group-item > .ant-checkbox input"));
            int iSizes = JobTypes.Count;
            for (int i = 0; i < iSizes; i++)
            {
                String Value2 = JobTypes.ElementAt(i).GetAttribute("value");
                if (Value2 == row.GetValue("JobTypes"))
                {
                    JobTypes.ElementAt(i).Click();
                    // This will take the execution out of for loop
                    break;
                }
            }
        }

        public string GetJobTitle()
        {
            Driver.WaitForDisplayed(By.XPath("(//span)[14]"));
            return JobTitle.GetInnerText(Driver);
        }

        public string DashboardPageTitle()
        {
            return Driver.Title;
        }

        //Scenario-Posting a job without entering mandatory field
        public void SaveWithoutMandatoryField()
        {
            Save.Click();
        }
             
      public bool AssertMandatoryField()
        {
            bool result = MandatoryTitle.Text.Contains("Please enter a title for the job") &&
                   MandatorySummary.Text.Contains("Please enter a job summary") &&
                   MandatoryDescription.Text.Contains("Please enter a job description") &&
                   MandatoryCategory.Text.Contains("Please enter the category") &&
                   MandatorySubCategory.Text.Contains("Please enter the sub-category") &&
                   MandatoryExperience.Text.Contains("Please enter the experience") &&
                   MandatoryQualification.Text.Contains("Please enter at least one qualification") &&
                   MandatoryVisaStatus.Text.Contains("Please enter at least one visa status") &&
                   MandatoryJobType.Text.Contains("Please enter at least one job type") &&
                   MandatoryCountry.Text.Contains("Please enter the country") &&
                   MandatoryCity.Text.Contains("Please enter the city") &&
                   MandatoryEndDate.Text.Contains("Please enter the end date");
             
            Console.WriteLine(result);


            if (result)
            {
                return true;
                
            }
            return false;
         
        }

        //Scenario-Posting the job outside the experience year range
        public void ExperienceOutsideExpectedRange(string YearsOfExperienceInput)
        {
            ExcelData row = ExcelUtil.DataSet.SelectSheet("ADD").GetRowByKey("Valid");
            Company.WaitForClickable(Driver);
            Company.SendKeys(row.GetValue("Company"));
            Title.SendKeys(row.GetValue("Title"));
            Summary.SendKeys(row.GetValue("Summary"));
            Description.SendKeys(row.GetValue("Description"));

            Category.SelectDropDownValue(Driver, row.GetValue("Category"));
            SubCategory.SelectDropDownValue(Driver, row.GetValue("SubCategory"));
            YearsOfExperience.SendKeys(YearsOfExperienceInput);

            CheckBoxGroups();

            Actions actions = new Actions(Driver);
            actions.ClickAndHold(SalaryRangeSlider);
            actions.MoveByOffset(2, 2);
            actions.Build().Perform();

            Country.SelectDropDownValue(Driver, row.GetValue("Country"));
            City.SelectDropDownValue(Driver, row.GetValue("City"));

            // Date fields
            StartDate.SelectDate(Driver, row.GetValue("StartDate"));
            EndDate.SelectDate(Driver, row.GetValue("EndDate"));
            ExpiryDate.SelectDate(Driver, row.GetValue("ExpiryDate"));
            Save.Click();
        }

        public string GetYears()
        {
            FirstJobInList.Click();
            return Actualyear.GetAttribute("value");
        }

        //Scenario-Posting a job with dates with invalid format
        public void DatesFormat(string JobStartDate, string JobEndDate, string JobExpiryDate)
        {
            ExcelData row = ExcelUtil.DataSet.SelectSheet("ADD").GetRowByKey("Valid");
            Company.WaitForClickable(Driver);
            Company.SendKeys(row.GetValue("Company"));
            Title.SendKeys(row.GetValue("Title"));
            Summary.SendKeys(row.GetValue("Summary"));
            Description.SendKeys(row.GetValue("Description"));

            Category.SelectDropDownValue(Driver, row.GetValue("Category"));
            SubCategory.SelectDropDownValue(Driver, row.GetValue("SubCategory"));
            YearsOfExperience.SendKeys(row.GetValue("YearsOfExperience"));
            CheckBoxGroups();


            Actions actions = new Actions(Driver);
            actions.ClickAndHold(SalaryRangeSlider);
            actions.MoveByOffset(2, 2);
            actions.Build().Perform();

            Country.SelectDropDownValue(Driver, row.GetValue("Country"));
            City.SelectDropDownValue(Driver, row.GetValue("City"));

            // Date fields
          
            StartDate.SelectDate(Driver, JobStartDate);
            EndDate.SelectDate(Driver, JobEndDate);
            ExpiryDate.SelectDate(Driver, JobExpiryDate);
            Save.Click();
        }

        public string GetDateInvalidFormat()
        {
            return InvalidFormatDate.GetInnerText(Driver);
        }

        
        //Scenario-Posting a job with PastDate
        public void ExpiredDate(string PastEndDate, string PastExpiryDate)
        {
            ExcelData row = ExcelUtil.DataSet.SelectSheet("ADD").GetRowByKey("Valid");
            Company.WaitForClickable(Driver);
            Company.SendKeys(row.GetValue("Company"));
            Title.SendKeys(row.GetValue("Title"));
            Summary.SendKeys(row.GetValue("Summary"));
            Description.SendKeys(row.GetValue("Description"));

            Category.SelectDropDownValue(Driver, row.GetValue("Category"));
            SubCategory.SelectDropDownValue(Driver, row.GetValue("SubCategory"));
            YearsOfExperience.SendKeys(row.GetValue("YearsOfExperience"));
            CheckBoxGroups();


            Actions actions = new Actions(Driver);
            actions.ClickAndHold(SalaryRangeSlider);
            actions.MoveByOffset(2, 2);
            actions.Build().Perform();

            Country.SelectDropDownValue(Driver, row.GetValue("Country"));
            City.SelectDropDownValue(Driver, row.GetValue("City"));

            // Date fields

            StartDate.SelectDate(Driver, row.GetValue("StartDate"));
            EndDate.SelectDate(Driver, PastEndDate);
            ExpiryDate.SelectDate(Driver, PastExpiryDate);
            Save.Click();
        }

        public string GetExpiredDate()
        {
            return InvalidFormatDate.GetInnerText(Driver);
        }
        
    }
}
        
    


      

    



       
         

    


