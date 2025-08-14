using System;
using System.Collections.Generic;
using System.IO;

namespace SchoolGradingSystem
{
    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public int Score { get; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80 && Score <= 100) return "A";
            if (Score >= 70 && Score <= 79) return "B";
            if (Score >= 60 && Score <= 69) return "C";
            if (Score >= 50 && Score <= 59) return "D";
            return "F";
        }
    }

    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();

            using (var reader = new StreamReader(inputFilePath))
            {
                string? line;
                int lineNumber = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    if (string.IsNullOrWhiteSpace(line))
                        continue; // Skip empty lines

                    var parts = line.Split(',');

                    if (parts.Length != 3)
                        throw new MissingFieldException($"Line {lineNumber}: Expected 3 fields, got {parts.Length}.");

                    try
                    {
                        int id = int.Parse(parts[0].Trim());
                        string fullName = parts[1].Trim();
                        if (string.IsNullOrWhiteSpace(fullName))
                            throw new MissingFieldException($"Line {lineNumber}: Name is missing.");

                        if (!int.TryParse(parts[2].Trim(), out int score))
                            throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid score format.");

                        if (score < 0 || score > 100)
                            throw new InvalidScoreFormatException($"Line {lineNumber}: Score must be between 0 and 100.");

                        students.Add(new Student(id, fullName, score));
                    }
                    catch (FormatException)
                    {
                        throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid ID format.");
                    }
                }
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using (var writer = new StreamWriter(outputFilePath))
            {
                foreach (var student in students)
                {
                    writer.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}");
                }
            }
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var processor = new StudentResultProcessor();

            Console.WriteLine("=== School Grading System ===");
            Console.Write("Enter input file path: ");
            string inputPath = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter output file path: ");
            string outputPath = Console.ReadLine() ?? string.Empty;

            try
            {
                var students = processor.ReadStudentsFromFile(inputPath);
                processor.WriteReportToFile(students, outputPath);

                Console.WriteLine($"Report successfully written to '{outputPath}'.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: The input file was not found.");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"Score Format Error: {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"Missing Field Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }
    }
}
