﻿using DocSpotApp.Models;
using DocSpotApp.Repository.DAL.Interfaces;
using DocSpotApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocSpotApp.Repository.DAL.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DocSpotDBContext _docSpotDBContext;


        public ApplicationUserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, DocSpotDBContext docSpotDBContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _docSpotDBContext = docSpotDBContext;
        }

        public async Task<List<PatientVM>> GetPatientsAsync()
        {
            var patients = new List<PatientVM>();

            var patientRole = await _roleManager.FindByNameAsync("Patient");
            if (patientRole == null)
            {
                return patients;
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(patientRole.Name);

            patients.AddRange(usersInRole.Select(user => new PatientVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Username = user.UserName,
                DOB = user.DOB,
                Email = user.Email,
                Mobile = user.PhoneNumber
            }));

            return patients;
        }

        public async Task<List<DoctorVM>> GetDoctorssAsync()
        {
            var doctors = new List<DoctorVM>();
            var departments = _docSpotDBContext.Departments;
            var hospitals = _docSpotDBContext.Hospitals;

            var Role = await _roleManager.FindByNameAsync("Doctor");
            if (Role == null)
            {
                return doctors;
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(Role.Name);

            doctors.AddRange(usersInRole.Select(user => new DoctorVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Username = user.UserName,
                DOB = user.DOB,
                Email = user.Email,
                Mobile = user.PhoneNumber,
                DepartmentId = user.DepartmentId,
                Fees = user.Fees,
                HospitalId = user.HospitalId,
                DepartmentName = departments.First(x => x.Id == user.DepartmentId).Name,
                HospitalName = hospitals.First(x => x.Id == user.HospitalId).Name,

            }));

            return doctors;
        }

       public async Task<List<DoctorVM>> GetDoctorListAsync(int hospitalId, int deptId)
{
    var doctors = new List<DoctorVM>();
    var departments = _docSpotDBContext.Departments;
    var hospitals = _docSpotDBContext.Hospitals;

    var Role = await _roleManager.FindByNameAsync("Doctor");
    if (Role == null)
    {
        return doctors;
    }

    var usersInRole = await _userManager.GetUsersInRoleAsync(Role.Name);

    doctors.AddRange(usersInRole
        .Where(user => user.HospitalId == hospitalId && user.DepartmentId == deptId)
        .Select(user => new DoctorVM
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Address = user.Address,
            Username = user.UserName,
            DOB = user.DOB,
            Email = user.Email,
            Mobile = user.PhoneNumber,
            DepartmentId = user.DepartmentId,
            Fees = user.Fees,
            HospitalId = user.HospitalId,
            DepartmentName = departments.FirstOrDefault(x => x.Id == user.DepartmentId)?.Name,
            HospitalName = hospitals.FirstOrDefault(x => x.Id == user.HospitalId)?.Name
        }));

    return doctors;
}

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id) ?? throw new KeyNotFoundException("User not found, try again later");

            user.Email = model.Email;
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.DepartmentId = model.DepartmentId;
            user.Fees = model.Fees;
            user.HospitalId = model.HospitalId;
            user.DOB = model.DOB;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        
        public async Task<bool> DeleteAsync(string userId)
        {
            var appointmentList = _docSpotDBContext.Appointments.Where(x => x.PatientId == userId || x.DoctorId == userId);
            foreach (var item in appointmentList)
            {
                item.IsDeleted = false;
            }
            _docSpotDBContext.UpdateRange(appointmentList);
            await _docSpotDBContext.SaveChangesAsync();
            var patient = await _userManager.FindByIdAsync(userId);
            if (patient == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(patient);
            if (result.Succeeded)
            {
                _docSpotDBContext.UpdateRange(appointmentList);
                await _docSpotDBContext.SaveChangesAsync();
                return result.Succeeded;
            }
            return result.Succeeded;
        }

    }

}

