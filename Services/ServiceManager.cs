using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Text.Json;
using System.Windows; 
using UIApplication.Models; 

namespace UIApplication.Services
{
    public static class ServiceManager 
    {
        private static string servicesFileName = "services.json";
        private static List<Service> _services;
        private static int _nextId = 1;

        static ServiceManager()
        {
            LoadServices();
        }

        private static void LoadServices()
        {
            if (File.Exists(servicesFileName))
            {
                string jsonString = File.ReadAllText(servicesFileName);
                _services = JsonSerializer.Deserialize<List<Service>>(jsonString);
                if (_services == null)
                {
                    _services = new List<Service>();
                }

                if (_services.Count > 0)
                {
                    int maxId = 0;
                    foreach (var service in _services)
                    {
                        if (service.Id > maxId)
                        {
                            maxId = service.Id;
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
                _services = new List<Service>();
                SaveServices();
            }
        }

        private static void SaveServices()
        {
            string jsonString = JsonSerializer.Serialize(_services);
            File.WriteAllText(servicesFileName, jsonString);
        }

        public static List<Service> GetAllServices()
        {
            List<Service> userService= new List<Service>();
            foreach (var appointment in _services)
            {
                userService.Add(appointment);
            }
            return userService;
        }

        public static Service GetServiceById(int id)
        {
            foreach (var service in _services)
            {
                if (service.Id == id)
                {
                    return service;
                }
            }
            return null;
        }

        public static bool AddService(Service service)
        {
            try
            {
                service.Id = _nextId;
                _nextId++;
                _services.Add(service);
                SaveServices();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool UpdateService(Service service)
        {
            Service existingService = null;
            foreach (var s in _services)
            {
                if (s.Id == service.Id)
                {
                    existingService = s;
                    break;
                }
            }

            if (existingService != null)
            {
                existingService.Name = service.Name;
                existingService.Description = service.Description;
                existingService.Price = service.Price;
                SaveServices();
                return true;
            }
            return false;
        }

        public static bool DeleteService(int id)
        {
            Service serviceToDelete = null;
            foreach (var service in _services)
            {
                if (service.Id == id)
                {
                    serviceToDelete = service;
                    break;
                }
            }

            if (serviceToDelete != null)
            {
                _services.Remove(serviceToDelete);
                SaveServices();
                return true;
            }
            return false;
        }
    }
}