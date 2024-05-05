using DocSpotApp.Models;
using DocSpotApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocSpotApp.Repository.DAL.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<List<PatientVM>> GetPatientsAsync();
        Task<List<DoctorVM>> GetDoctorssAsync();
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<bool> UpdateAsync(ApplicationUser user);
        Task<bool> DeleteAsync(string userId);

    }
}
