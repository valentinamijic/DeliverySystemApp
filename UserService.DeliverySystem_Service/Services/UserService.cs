using AutoMapper;
using DeliverySystem_Common.DTOs.User;
using DeliverySystem_Common.Enums;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DeliverySystem_DAL.Abstract.Repositories;
using UserService.DeliverySystem_DAL.Abstract.Services;

namespace UserService.DeliverySystem_BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;

        public UserService(IMapper mapper, IUserRepository userRepo)
        {
            _mapper = mapper;
            _userRepo = userRepo;
        }

        public KeyValuePair<ReturnValue, LoggedDto> FindUser(string email)
        {
            if(!String.IsNullOrWhiteSpace(email))
            {
                LoggedDto loggedUser = _userRepo.FindUserByEmail(email);
                if (loggedUser == null) return new KeyValuePair<ReturnValue, LoggedDto>(ReturnValue.ERROR_OCCURED, null);

                return new KeyValuePair<ReturnValue, LoggedDto>(ReturnValue.OK, loggedUser);
            }
            return new KeyValuePair<ReturnValue, LoggedDto>(ReturnValue.ERROR_OCCURED, null);
        }

        public KeyValuePair<ReturnValue, bool> HandleNameChange(NameHandleDto nameDto)
        {
            if (String.IsNullOrWhiteSpace(nameDto.Email) || String.IsNullOrWhiteSpace(nameDto.Name))
                return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);

            LoggedDto loggedUser = _userRepo.FindUserByEmail(nameDto.Email);
            if (loggedUser == null) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);

            bool retVal =  _userRepo.ChangeName(nameDto);

            if (!retVal) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
            return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
        }

        public KeyValuePair<ReturnValue, bool> HandleUsernameChange(UsernameHandleDto usernameDto)
        {
            if (String.IsNullOrWhiteSpace(usernameDto.Email) || String.IsNullOrWhiteSpace(usernameDto.Username))
                return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);

            LoggedDto loggedUser = _userRepo.FindUserByEmail(usernameDto.Email);
            if (loggedUser == null) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);

            bool exists = _userRepo.CheckIfUsernameExists(usernameDto.Username);
            if (exists) return new KeyValuePair<ReturnValue, bool>(ReturnValue.USERNAME_EXISTS, false);

            bool retVal = _userRepo.ChangeUsername(usernameDto);

            if(!retVal) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
            return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
        }

        public KeyValuePair<ReturnValue, bool> HandleLastnameChange(LastnameHandleDto lastnameDto)
        {
            if (String.IsNullOrWhiteSpace(lastnameDto.Email) || String.IsNullOrWhiteSpace(lastnameDto.Lastname))
                return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);

            LoggedDto loggedUser = _userRepo.FindUserByEmail(lastnameDto.Email);
            if (loggedUser == null) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);

            bool retVal = _userRepo.ChangeLastname(lastnameDto);

            if (!retVal) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
            return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
        }

        public KeyValuePair<ReturnValue, bool> HandleAddressChange(AddressHandleDto addressDto)
        {
            if (String.IsNullOrWhiteSpace(addressDto.Email) || String.IsNullOrWhiteSpace(addressDto.Address))
                return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);

            LoggedDto loggedUser = _userRepo.FindUserByEmail(addressDto.Email);
            if (loggedUser == null) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);

            bool retVal = _userRepo.ChangeAddress(addressDto);

            if (!retVal) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
            return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
        }

        public KeyValuePair<ReturnValue, bool> HandleDateChange(DateHandleDto dateDto)
        {
            if (String.IsNullOrWhiteSpace(dateDto.Email) || DateTime.Compare(dateDto.Date, new DateTime()) == 0)
                return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);

            LoggedDto loggedUser = _userRepo.FindUserByEmail(dateDto.Email);
            if (loggedUser == null) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);

            bool retVal = _userRepo.ChangeDate(dateDto);

            if (!retVal) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
            return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
        }

        public KeyValuePair<ReturnValue, bool> HandlePasswordChange(PasswordHandleDto passwordDto)
        {
            if (String.IsNullOrWhiteSpace(passwordDto.Email) || String.IsNullOrWhiteSpace(passwordDto.Password) 
                || String.IsNullOrWhiteSpace(passwordDto.OldPassword))
                return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);

            LoggedDto loggedUser = _userRepo.FindUserByEmail(passwordDto.Email);
            if (loggedUser == null) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);

            string password = _userRepo.FindPassword(passwordDto.Email);
            if (password != null)
            {
                if (BCrypt.Net.BCrypt.Verify(passwordDto.OldPassword, password))
                {
                    HashPassword(passwordDto.Password, out string passwordHash);

                    bool retVal = _userRepo.ChangePassword(passwordDto, passwordHash);

                    if (!retVal) return new KeyValuePair<ReturnValue, bool>(ReturnValue.INVALID_PASSWORD, false);
                    return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
                }
            }
            return new KeyValuePair<ReturnValue, bool>(ReturnValue.INVALID_PASSWORD, false);
        }

        public KeyValuePair<ReturnValue, bool> HandleUsernameAdd(UsernameHandleDto usernameHandle)
        {
            if (String.IsNullOrWhiteSpace(usernameHandle.Email) || String.IsNullOrWhiteSpace(usernameHandle.Username))
                return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);

            LoggedDto loggedUser = _userRepo.FindUserByEmail(usernameHandle.Email);
            if (loggedUser == null) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);

            bool exists = _userRepo.CheckIfUsernameExists(usernameHandle.Username);
            if (exists) return new KeyValuePair<ReturnValue, bool>(ReturnValue.USERNAME_EXISTS, false);

            bool retVal = _userRepo.AddUsername(usernameHandle);

            if (!retVal) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
            return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
        }

        public KeyValuePair<ReturnValue, bool> HandleAddressAdd(AddressHandleDto addressHandle)
        {
            if (String.IsNullOrWhiteSpace(addressHandle.Email) || String.IsNullOrWhiteSpace(addressHandle.Address))
                return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);

            LoggedDto loggedUser = _userRepo.FindUserByEmail(addressHandle.Email);
            if (loggedUser == null) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);

            bool retVal = _userRepo.AddAddress(addressHandle);

            if (!retVal) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
            return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
        }

        public KeyValuePair<ReturnValue, DateTime?> HandleDateAdd(DateHandleDto dateDto)
        {
            if (String.IsNullOrWhiteSpace(dateDto.Email) || DateTime.Compare(dateDto.Date, new DateTime()) == 0)
                return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.EMPTY_FIELDS, null);

            LoggedDto loggedUser = _userRepo.FindUserByEmail(dateDto.Email);
            if (loggedUser == null) return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.ERROR_OCCURED, null);

            DateTime? date = _userRepo.AddDate(dateDto);
            if (date == null) return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.ERROR_OCCURED, null);

            return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.OK, date);
        }

        public List<DelivererDto> FindDeliverers()
        {
           return _userRepo.AllDeliverers();
        }

        public KeyValuePair<ReturnValue, bool> VerifyUser(VerifyDto verifyDto)
        {
            if (!String.IsNullOrWhiteSpace(verifyDto.Email))
            {
                bool? oldState = null;
                LoggedDto loggedUser = _userRepo.FindUserByEmail(verifyDto.Email);
                if (loggedUser == null) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);

                oldState = _userRepo.GetAcceptance(verifyDto.Email);

                if (oldState != true)
                {
                    if (_userRepo.VerifyUser(verifyDto))
                    {
                        NotifyDelivererAboutVerify(verifyDto.Email, loggedUser.Username);
                        return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
                    }
                    return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
                }
                return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);

            }
            return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
        }

        public KeyValuePair<ReturnValue, bool> RejectUser(VerifyDto verifyDto)
        {
            if (!String.IsNullOrWhiteSpace(verifyDto.Email))
            {
                bool? oldState = null;
                LoggedDto loggedUser = _userRepo.FindUserByEmail(verifyDto.Email);
                if (loggedUser == null) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
              
                oldState = _userRepo.GetAcceptance(verifyDto.Email);

                if (oldState != false)
                {
                    if (_userRepo.RejectUser(verifyDto))
                    {
                        NotifyDelivererAboutRejection(verifyDto.Email);
                        return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
                    }
                    return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false); 
                }
                return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
            }
            return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
        }

        public KeyValuePair<ReturnValue, bool> HandlePhotoUpload(byte[]? ImageData, string? ImageMimeType, string email)
        {
            if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(ImageMimeType))
                return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);

            bool retVal = _userRepo.AddPhoto(ImageData, ImageMimeType, email);

            if (!retVal) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);
            return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
        }





        private void HashPassword(string password, out string passwordHash)
        {
            passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
        private void NotifyDelivererAboutVerify(string email, string username)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse("valentinamijic@gmail.com"));
            message.To.Add(MailboxAddress.Parse("valentinamijic@gmail.com")); //here goes actual mail
            message.Subject = "DeliverySystemApp - REGISTRATION ACCEPTED";

            message.Body = new TextPart(TextFormat.Html)
            {
                Text =
                "Dear " + username +
                ", <br /> <br />  Your registration has been accepted." +
                "<br /> <br /> You can now sign into your account and enjoy the services provided by DeliverySystemApp!" +
                "<br /> <br /> <br /> DeliverySystemApp Admin Team"
            };

            using var mail = new SmtpClient();
            mail.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            mail.Authenticate("valentinamijic@gmail.com", "rxiljvovoxhxqfty");
            mail.Send(message);
            mail.Disconnect(true);
        }
        private void NotifyDelivererAboutRejection(string email)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse("valentinamijic@gmail.com"));
            message.To.Add(MailboxAddress.Parse("valentinamijic@gmail.com")); //here goes actual mail
            message.Subject = "DeliverySystemApp - REGISTRATION REJECTED";

            message.Body = new TextPart(TextFormat.Html)
            {
                Text =
                "Dear " + email +
                ", <br /><br />  Your registration has been rejected." +
                "<br /> You can still register as a customer and enjoy the services provided by DeliverySystemApp!" +
                "<br /> <br /><br />  DeliverySystemApp Admin Team"
            };

            using var mail = new SmtpClient();
            mail.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            mail.Authenticate("valentinamijic@gmail.com", "rxiljvovoxhxqfty");
            mail.Send(message);
            mail.Disconnect(true);
        }
    }
}
