
function onChangeStatusReasons() {
    debugger;
    try {
        var caseid = Xrm.Page.getAttribute("ticketnumber").getValue();
        var status = Xrm.Page.getAttribute("statuscode").getValue();
        var statusString = status.toString();
        var lookupObj = Xrm.Page.getAttribute("customerid"); //Check for Lookup Object
        if (lookupObj !== null) {
            var lookupObjValue = lookupObj.getValue();//Check for Lookup Value
            if (lookupObjValue !== null) {
                var contactid = lookupObjValue[0].id;
                var _contactId = contactid.replace("{", "").replace("}", "");

                var registrationId = getRegistrationId(_contactId);// To get record
                if (registrationId !== null) {
                    if (status === 167490008 || status === 167490009 || status === 167490025) {

                        var data = JSON.stringify({
                            "Source": {
                                "caseid": caseid,
                                "contactid": _contactId,
                                "regId": registrationId,
                                "NotificationType": statusString
                            }
                        });
                      
                       

                        var xhr = new XMLHttpRequest();
                        xhr.withCredentials = true;
                        xhr.addEventListener("readystatechange", function () {
                            if (this.readyState === 4) {
                                if (this.status === 200 || this.status===204) {
                                    var results = JSON.parse(xhr.responseText);
                                    //  successCallback(results);
                                }
                            }
                        });

                        xhr.open("POST", "https://77.69.181.239/api/Notification/PostNotification");
                        xhr.setRequestHeader("Content-Type", "application/json");
                        xhr.setRequestHeader("Authorization", "Basic VFJBQE1vYmlsZTIwMTk6IzIwMTkmQVBJVFJB");
                        xhr.send(data);
                    }
                }
            }

        }
    }
    catch (e) {
        alert(e);
    }
}

function getRegistrationId(contactid) {
    debugger;
    try {
    

        var tra_registrationid;
        var req = new XMLHttpRequest();
        req.open("GET", Xrm.Page.context.getClientUrl() + "/api/data/v8.2/contacts(" + contactid + ")?$select=tra_registrationid", false);
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("Prefer", "odata.include-annotations=\"*\"");
        req.onreadystatechange = function () {
            if (this.readyState === 4) {
                req.onreadystatechange = null;
                if (this.status === 200) {
                    var result = JSON.parse(this.response);
                    tra_registrationid = result["tra_registrationid"];
                } else {
                    Xrm.Utility.alertDialog(this.statusText);
                }
            }
        };
        req.send();
    }
    catch (e) {

        alert(e);
    }
    return tra_registrationid;
}
