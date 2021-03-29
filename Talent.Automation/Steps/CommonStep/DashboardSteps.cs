using MVPStudio.Framework.Extensions;
using OpenQA.Selenium;
using Talent.Automation.Page;
using Talent.Automation.Steps.BaseStep;
using TechTalk.SpecFlow;

namespace Talent.Automation.Steps
{
    [Binding]
    public class DashboardSteps : Base
    {
        private readonly ScenarioContext context;

        public DashboardSteps(IWebDriver driver, ScenarioContext injectedContext) : base(driver)
        {
            context = injectedContext;
        }

        [Given(@"I am on Dashboard Page")]
        public void GivenIAmOnDashboardPage()
        {
            CurrentPage = GetInstance<DashboardPage>(Driver);
        }

        [Given(@"I am on Job Post page")]
        public void GivenIAmOnJobPostPage()
        {
            CurrentPage = GetInstance<DashboardPage>(Driver);
            CurrentPage = CurrentPage.As<DashboardPage>().GotoPostJobs();
        }


        [When(@"I click logout button")]
        public void WhenIClickLogoutButton()
        {
            CurrentPage = CurrentPage.As<DashboardPage>().Logout();
        }

        [When(@"I click on Profile menu")]
        public void WhenIClickOnProfileMenu()
        {
            CurrentPage = CurrentPage.As<DashboardPage>().GoToProfilePage();
            Driver.WaitForPageLoaded("profile");
        }

        [When(@"I click on Jobs watch list menu")]
        public void WhenIClickOnJobsWatchListMenu()
        {
            CurrentPage = CurrentPage.As<DashboardPage>().GoToJobsWatchListPage();
        }

        [When(@"I click on Talent Feed menu")]
        public void WhenIClickOnTalentFeedMenu()
        {
            CurrentPage = CurrentPage.As<DashboardPage>().GoToTalentFeedPage();
        }

        [When(@"I click on Jobs menu")]
        public void WhenIClickOnJobsMenu()
        {
            CurrentPage = CurrentPage.As<DashboardPage>().GotoJobsPage();
        }

    }
}
