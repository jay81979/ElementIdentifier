using System;
using TechTalk.SpecFlow;
using Jay8TestSuite.AcceptanceTests.StepHelpers;

namespace Jay8TestSuite.AcceptanceTests.Steps
{
    [Binding]
    public class ElementHighlightSteps : BaseSteps
    {
        [Given(@"I enter the url ""(.*)""")]
public void GivenIEnterTheUrl(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I click ""(.*)""")]
        public void WhenIClick(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I hover mouse at x ""(.*)"" and y ""(.*)""")]
        public void WhenIHoverMouseAtXAndY(int p0, int p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
