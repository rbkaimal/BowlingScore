# Bowling score
  * Implement a simple bowling score calculator, using the traditional scoring method specified here https://en.wikipedia.org/wiki/Ten-pin_bowling#Traditional_scoring.
# Installing
  * Requires .netCore 3.1  and requires Microsoft.AspNetCore.Mvc.Newtonsoft as additional  packge  needed
  * xunit used for unit testing
  * Moq framework is used for mocking 

# Design
  * A simple REST api to return progress score
  * The input is the list of pins downed on each throw. A throw can have a value of 0 (no pinsdowned) to 10 (all pins down).
  * The output is the current score based on the pins thrown, the progress score of each framealready completed, as well as an indication as to whether the game is finished
  
  

# Sample Request
  * Post request to {ApiBaseUrl}/scores
  * {   "pinsDowned": [1,1,1,1,9,1,2,8,9,1,10,10]}
  
# Unit Testing
  * Unit testing completed for the  classes. 
  
# Improvements possible
  * The score once calculated can be cached  if needed
  * The API end point can be protected to add security
  *

