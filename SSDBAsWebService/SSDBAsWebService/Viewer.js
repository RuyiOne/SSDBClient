/// <reference path="jquery-1.8.2.min.js" />

Viewer = new Object();
Viewer = $(window)

Viewer.ready(function () {
   Viewer.ip = ["127.0.0.1"];
   Viewer.port = ["8888"];
   Viewer.sname = ["Default"];
   var urlc = (window.location + "SSDB").replace('/#', '/');    //"http://localhost/ssdb.com/SSDB";   
   var $ip, $port, $tablelength = null;
   
   Viewer.addconnection = function (sname, ip, port, jdata) {
        $ip = ip;
        $port = port;
        
            $.ajax({
                type: "POST",
                cache: false,
                url: urlc + "/GetInfo", //change this finally
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ipadd: $ip, portno: $port}),
                success: function (Json_Data) {
                    i = Viewer.ip.length
                    Viewer.sname.push(sname);
                    Viewer.ip.push($ip);
                    Viewer.port.push($port);                   
                    jdata.push(Json_Data);                    
                },
                error: function (xhr, status, error) {
                    alert(status + ': ' + error);
                    confirm("the ssdb server "+ $ip + ":" + $port +  " is not available or not configured in this system please add another");
                }
            });
            
    };   
   Viewer.refresh = function (callback, i) {
       //alert($ip + ':' + $port)
       $ip = Viewer.ip[i];
       $port = Viewer.port[i];
       $.ajax({
           type: "POST",
           cache: false,
           url: urlc + "/GetInfo",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           data: JSON.stringify({ ipadd: $ip, portno: $port }),
           success: function (Json_Data) {
               callback(Json_Data, $ip, $port);
           },
           error: function (xhr, status, error) {
               alert(status + ': ' + error);
               confirm("the ssdb server " + $ip + ":" + $port + " is not available or not configured in this system please add another");
           }
       });


   };
   Viewer.delete = function (name) {
       for (i = 1; i < Viewer.sname.length; i++) {
           if (Viewer.sname[i] === name) {
               Viewer.sname[i] === null;
               Viewer.ip[i] = null;
               Viewer.port[i] = null;
               for (i = 1; i < Viewer.sname.length-1; i++) {
                   var $ipc = ["127.0.0.1"]; var $iport = ["8888"]; var $sname = ["Default"];
                   if (Viewer.sname[i]) {
                       $sname.push(Viewer.sname[i]); $ipc.push(Viewer.ip[i]); $iport.push(Viewer.port[i])
                   }
                   
               }               
           }
           else {
               
           }
       }

       Viewer.sname[i] === $sname;
       Viewer.ip[i] = $ipc;
       Viewer.port[i] = $iport;
   };
   Viewer.connection = function (jdata) {
       $.ajax({
           type: "GET",
           cache: false,
           url: urlc + "/GetDetails",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           data: "{}",
           success: function (Json_Data) {
               $('body').append('<div id ="new" style ="margin-left:0px;"></div>');
               $('#new').append('<table id="infonew" style="width:400px"></table>');

               for (var i = 0; i < Json_Data.length - 1; i++) {
                   $('#infonew').append('<tr><td>' + Json_Data[i] + '</td></tr>');
               }
           },
           error: function (xhr, status, error) { alert(status + ': ' + error); }
       });

   };
   Viewer.default = function (jdata, callback) {
       
       $.ajax({
           type: "GET",
           cache: false,
           url: urlc + "/Info",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           data: "{}",
           success: function (Json_Data) {
               $ip = Viewer.ip[0]; $port = Viewer.port[0]; jdata.push(Json_Data);
               callback(Json_Data, Viewer.ip[0], Viewer.port[0]);
           },
           error: function (xhr, status, error) {
               alert(status + ': ' + error);
               confirm("the localhost ssdb server 127.0.0.1:8888 is not available or not configured in this system please add another");
           }
            
           
       });

       
   };
   Viewer.view = function (callback, length) {
       $tablelength = length;
       $.ajax({
           type: "POST",
           cache: false,
           url: urlc + "/GetDetails",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           data: JSON.stringify({ ipadd: $ip, portno: $port, tablelength: $tablelength }),
           success: function (Json_Data) {            
               callback(Json_Data);
           },
           error: function (xhr, status, error) {
               alert(status + ': ' + error);
               confirm("the localhost ssdb server 127.0.0.1:6379 is not available or not configured in this system please add another");
           }
       });
   };
   Viewer.getkey = function (callback, type, key, pagesize, fstitem, lstitem) {       
       $.ajax({
           type: "POST",
           cache: false,
           url: urlc + "/GetKey",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           data: JSON.stringify({ datatype: type, keyname: key, size: pagesize, firstitem: fstitem, lastitem: lstitem}),
           success: function (Json_Data) {
               callback(Json_Data, type);
           },
           error: function (xhr, status, error) {
               alert(status + ': ' + error);
               confirm("the localhost ssdb server 127.0.0.1:8888 is not available or not configured in this system please add another");
           }
       });
   };
   Viewer.request = function (callback, callbackargs, request, args) {
       $.ajax({
           type: "POST",
           cache: false,
           url: urlc + "/GetRequest", //change this finally
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           data: JSON.stringify({ cmd: request, parameters: args}),
           success: function (Json_Data) {
               (Json_Data[0]) ? callback(callbackargs) : callback(null);
           },
           error: function (xhr, status, error) {
               alert(status + ': ' + error);
               confirm("the ssdb server " + $ip + ":" + $port + " is not available or not configured in this system please add another");
           }
       });
   };
});