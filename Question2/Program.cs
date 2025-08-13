using System;
using System.Collections.Generic;
using System.Linq;

namespace Question2
{
    public class Repository<T>
    {
        private readonly List<T> items = new();

        public void Add(T item) => items.Add(item);

        public List<T> GetAll() => new(items); 

        public T? GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }

    public class Patient
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Age = age;
            Gender = gender ?? throw new ArgumentNullException(nameof(gender));
        }

        public override string ToString() => $"[{Id}] {Name}, Age: {Age}, Gender: {Gender}";
    }

    public class Prescription
    {
        public int Id { get; }
        public int PatientId { get; }
        public string MedicationName { get; }
        public DateTime DateIssued { get; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName ?? throw new ArgumentNullException(nameof(medicationName));
            DateIssued = dateIssued;
        }

        public override string ToString() => $"[{Id}] {MedicationName} (Issued: {DateIssued:yyyy-MM-dd})";
    }

    public class HealthSystemApp
    {
        private readonly Repository<Patient> _patientRepo = new();
        private readonly Repository<Prescription> _prescriptionRepo = new();
        private readonly Dictionary<int, List<Prescription>> _prescriptionMap = new();

        public void SeedData()
        {
            // Patients
            _patientRepo.Add(new Patient(1, "Yaa Asantewaa", 34, "Female"));
            _patientRepo.Add(new Patient(2, "Nana Kwame", 45, "Male"));
            _patientRepo.Add(new Patient(3, "Kweku Obo", 29, "Female"));

            // Prescriptions
            _prescriptionRepo.Add(new Prescription(1, 1, "Amoxicillin", DateTime.Today.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(2, 1, "Ibuprofen", DateTime.Today.AddDays(-2)));
            _prescriptionRepo.Add(new Prescription(3, 2, "Metformin", DateTime.Today.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(4, 3, "Vitamin D", DateTime.Today.AddDays(-1)));
            _prescriptionRepo.Add(new Prescription(5, 1, "Paracetamol", DateTime.Today));
        }

        public void BuildPrescriptionMap()
        {
            _prescriptionMap.Clear();

            foreach (var prescription in _prescriptionRepo.GetAll())
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    _prescriptionMap[prescription.PatientId] = new List<Prescription>();
                }
                _prescriptionMap[prescription.PatientId].Add(prescription);
            }
        }

        public void PrintAllPatients()
        {
            Console.WriteLine("=== All Patients ===");
            foreach (var patient in _patientRepo.GetAll())
            {
                Console.WriteLine(patient);
            }
            Console.WriteLine();
        }

        public void PrintPrescriptionsForPatient(int patientId)
        {
            if (!_prescriptionMap.ContainsKey(patientId))
            {
                Console.WriteLine($"No prescriptions found for Patient ID {patientId}");
                return;
            }

            var patient = _patientRepo.GetById(p => p.Id == patientId);
            if (patient == null)
            {
                Console.WriteLine($"Patient ID {patientId} not found.");
                return;
            }

            Console.WriteLine($"\n=== Prescriptions for {patient.Name} ===");
            foreach (var prescription in _prescriptionMap[patientId])
            {
                Console.WriteLine(prescription);
            }
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var app = new HealthSystemApp();

            app.SeedData();
        app.BuildPrescriptionMap();

            app.PrintAllPatients();

            Console.Write("Enter a Patient ID to view prescriptions: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                app.PrintPrescriptionsForPatient(patientId);
            }
            else
            {
                Console.WriteLine("Invalid ID entered.");
            }
        }
    }
}
