function AddCalendarAppointment(appointmentSubject, appointmentBody, travelLocation, departDate, returnDate) {

    outlookApp = new ActiveXObject("Outlook.Application");
    nameSpace = outlookApp.getNameSpace("MAPI");

    // Get a handle of the Calendar folder
    apptFolder = nameSpace.getDefaultFolder(9);

    // Create a new Appointment item and fill it in
    apptItem = apptFolder.Items.add("IPM.Appointment");

    apptItem.Subject = appointmentSubject;
    apptItem.Body = appointmentBody;

    apptItem.Location = travelLocation;

    apptItem.Start = departDate;
    apptItem.End = returnDate;
    apptItem.Save();

    //apptItem.ReminderSet = True;
    //apptItem.ReminderMinutesBeforeStart = 15
    //apptItem.BusyStatus = Outlook.OlBusyStatus.olOutOfOffice;
    apptItem.AllDayEvent = true;

    // Show the new contact
    //apptItem.Display(false); // true = modal
}

function createDateAsUTC(date) {
    return new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds()));
}

function convertDateToUTC(date) {
    return new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
}
function SetTimeForAppointment(date) {

    var month = date.getMonth();
    var day = date.getDate();
    var year = date.getFullYear();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var sTime = '';
    if (hour > 12)
        sTime = hour.toString() + ":" + minute.toString() + "AM";
    else
        sTime = hour.toString() + ":" + minute.toString() + "PM";
    return formattedDate = (month + 1) + "/" + day + "/" + year + " " + sTime; //" 08:00AM";
}
