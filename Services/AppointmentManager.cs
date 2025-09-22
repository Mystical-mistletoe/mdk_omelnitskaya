using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Text.Json;
using System.Windows; 
using UIApplication.Models; 

namespace UIApplication.Services
{
    public static class AppointmentManager 
    {
        private static string appointmentsFileName = "appointments.json";
        private static List<Appointment> _appointments;
        private static int _nextId = 1;

        static AppointmentManager()
        {
            LoadAppointments();
        }

        private static void LoadAppointments()
        {
            if (File.Exists(appointmentsFileName))
            {
                string jsonString = File.ReadAllText(appointmentsFileName);
                _appointments = JsonSerializer.Deserialize<List<Appointment>>(jsonString);
                if (_appointments == null)
                {
                    _appointments = new List<Appointment>();
                }

                if (_appointments.Count > 0)
                {
                    int maxId = 0;
                    foreach (var appointment in _appointments)
                    {
                        if (appointment.Id > maxId)
                        {
                            maxId = appointment.Id;
                        }
                    }
                    _nextId = maxId + 1;
                }
                else
                {
                    _nextId = 1;
                }
            }
            else
            {
                _appointments = new List<Appointment>();
                SaveAppointments();
            }
        }

        private static void SaveAppointments()
        {
            string jsonString = JsonSerializer.Serialize(_appointments);
            File.WriteAllText(appointmentsFileName, jsonString);
        }

        public static List<Appointment> GetUserAppointments(string userLogin)
        {
            List<Appointment> userAppointments = new List<Appointment>();
            foreach (var appointment in _appointments)
            {
                if (appointment.UserLogin == userLogin)
                {
                    userAppointments.Add(appointment);
                }
            }
            return userAppointments;
        }

        public static List<Appointment> GetAllAppointments()
        {
            return _appointments;
        }

        public static bool AddAppointment(Appointment appointment)
        {
            try
            {
                //Проверка на дублирование
                bool isDuplicate = false;
                foreach (var app in _appointments)
                {
                    if (app.ServiceId == appointment.ServiceId && app.AppointmentTime == appointment.AppointmentTime)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if (isDuplicate)
                {
                    return false;
                }

                appointment.Id = _nextId;
                _nextId++;
                _appointments.Add(appointment);
                SaveAppointments();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ExportAppointmentsToTxt(string filePath)
        {
            List<string> lines = new List<string>();
            foreach (var appointment in _appointments)
            {
                lines.Add($"ID: {appointment.Id}, User: {appointment.UserLogin}, Service ID: {appointment.ServiceId}, Time: {appointment.AppointmentTime}");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
