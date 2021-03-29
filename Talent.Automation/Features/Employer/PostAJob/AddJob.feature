Feature: Posting a job as Employer
	As a Employer
	I should be able to post a new job

# Assumption here is that since this feature is specifically for a employer
# User is assumed to be already logged in as employer
Background: Login as employer
	
    Given I login as 'employer'
	And I am on Job Post page
	

#------------------------------------------------------------------------------------------------
@employer @jobs
Scenario: Posting a new job with valid data
    When I post a new job
	Then the job management page displays the newly added job


#------------------------------------------------------------------------------------------------
@employer @jobs
Scenario: Cancel posting a job
	When I cancel posting a new job
	Then I should be navigated to Dashboard page


#------------------------------------------------------------------------------------------------
@employer @jobs
Scenario Outline: Posting a job outside the expected range for years of experience
	When I fill out a job post with a requirement of <YearsOfExperienceInput> years of experience
	Then The years of experience is converted to <SavedYearsOfExperience>

Examples:
	| YearsOfExperienceInput | SavedYearsOfExperience |
	| -1					 | 0                      |
	| 26                     | 25                     |


#------------------------------------------------------------------------------------------------
@employer @jobs
Scenario: Posting a job without entering mandatory fields
	When I post a new job without entering any data
	Then all the mandatory fields are highlighted in red with appropriate error messages

#------------------------------------------------------------------------------------------------
@employer @jobs
Scenario Outline: Posting a job with dates with invalid format
	When I fill out a job post with the following details:
		| StartDate      | EndDate      | ExpiryDate      |
		| <JobStartDate> | <JobEndDate> | <JobExpiryDate> |
	Then those date fields are not changed

Examples:
	| JobStartDate | JobEndDate | JobExpiryDate |
	| 202-01-01    | 202-12-31  | 202-12-31     |
	| 20-01-01     | 20-12-31   | 20-12-31      |
	| 01-01-2020   | 31-12-2020 | 31-12-2020    |
	| 2020/01/01   | 2020/12/31 | 2020/12/31    |
	| 2020-13-31   | 2020-13-31 | 2020-13-31    |
	| 2020-00-01   | 2020-00-01 | 2020-00-01    |
	| 2020-01-32   | 2020-12-32 | 2020-12-32    |
	
#-------------------------------------------------------------------------------------------------------
@employer @jobs
Scenario Outline: Posting a job with Past End date and Past Expiry date 
When I post a new job with following ExpiryDate
|PastEndDate  |PastExpiryDate  | 
| 2020-08-11  | 2020-08-10     |
Then the date was disable and could not save the details with expiry date


 
