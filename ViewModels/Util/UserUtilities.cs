using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPYourNoteLibrary.Domain.Contract;
using UWPYourNoteLibrary.Domain;
namespace UWPYourNote.ViewModels.Util
{
    public class UserUtilities
    {
        public static void CheckIfUsersExists(string email, ICallback<CheckIfUserExistsUseCaseResponse> callback)
        {
            CheckIfUserExistsUseCaseRequest request = new CheckIfUserExistsUseCaseRequest();
            request.Email = email;
            CheckIfUserExistsUseCase checkIfUserExistsUseCase = new CheckIfUserExistsUseCase(request, callback);
            checkIfUserExistsUseCase.Execute();
        }
    }
}
