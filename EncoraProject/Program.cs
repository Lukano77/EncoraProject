using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

class Program
{
    static void Main()
    {
        string jsonFilePath = "test_result.json";
        // Path to the directory where the CSV file will be exported
        string exportDirectory = @"C:\"; ;

        try
        {
            // Read the JSON file
            string jsonContent = File.ReadAllText(jsonFilePath);
            // Deserialize JSON to a list of TestResult objects
            List<TestResult> testResults = JsonConvert.DeserializeObject<List<TestResult>>(jsonContent);

            // Calculate and display metrics
            int totalTestCases = testResults.Count;
            int passedTestCases = testResults.Count(result => result.Status == "Pass");
            int failedTestCases = testResults.Count(result => result.Status == "Fail");

            double totalExecutionTime = testResults.Sum(result => Convert.ToDouble(result.ExecutionTime));
            double averageExecutionTime = totalExecutionTime / totalTestCases;
            double minExecutionTime = testResults.Min(result => Convert.ToDouble(result.ExecutionTime));
            double maxExecutionTime = testResults.Max(result => Convert.ToDouble(result.ExecutionTime));

            Console.WriteLine($"Total number of test cases executed: {totalTestCases}");
            Console.WriteLine($"Number of test cases passed: {passedTestCases}");
            Console.WriteLine($"Number of test cases failed: {failedTestCases}");
            Console.WriteLine($"Average execution time for all test cases: {averageExecutionTime:F2} seconds");
            Console.WriteLine($"Minimum execution time among all test cases: {minExecutionTime:F2} seconds");
            Console.WriteLine($"Maximum execution time among all test cases: {maxExecutionTime:F2} seconds");


            // Write test results to CSV
            string csvFilePath = Path.Combine(exportDirectory, "test_results.csv");
            WriteToCSV(testResults, csvFilePath);

            Console.WriteLine($"CSV file has been created successfully at: {csvFilePath}");


        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    static void WriteToCSV(List<TestResult> testResults, string csvFilePath)
    {
        using (StreamWriter writer = new StreamWriter(csvFilePath))
        {
            // Write header
            writer.WriteLine("Test Case Name,Status,Execution Time,Timestamp");

            // Write each test result
            foreach (TestResult result in testResults)
            {
                // Format timestamp
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                // Write data
                writer.WriteLine($"{result.TestCase},{result.Status},{result.ExecutionTime},{timestamp}");
            }
        }
    }


}




// Define a class to represent a test result
class TestResult
{
    // Assumed JSON format/structure for test result:
    // {
    //     "TestCase": "Test case name",
    //     "Status": "Pass/Fail",
    //     "ExecutionTime": "Time taken for execution"
    // }

    public string TestCase { get; set; }
    public string Status { get; set; }
    public string ExecutionTime { get; set; }
}
