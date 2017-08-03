var express = require('express');
var fs = require("fs");
var mongoClient = require('mongodb').MongoClient;
var ObjectId = require('mongodb').ObjectId;
var bodyParser = require('body-parser');
var app = express();
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

var database = {};
var CONTACT_COLLECTION_NAME = "Contact";
var MONGODB_LOCATION = "mongodb://localhost:27017/ContactManagement"

app.get('/contacts', function (req, res) {
    console.log("/contacts request received");

    var contacts = database.collection(CONTACT_COLLECTION_NAME);
    contacts.find().toArray(function (err, items) {
        res.setHeader('Content-Type', 'application/json');
        var myJSON = JSON.stringify(items);
        res.end(myJSON);
    });
});

app.get('/contacts/:id', function (req, res) {
    console.log("/contacts/:id request received");

    var contacts = database.collection(CONTACT_COLLECTION_NAME);
    var objectId = new ObjectId(req.params["id"]);
    contacts.findOne({ "_id": objectId }, function (err, items) {
        res.setHeader('Content-Type', 'application/json');
        var myJSON = JSON.stringify(items);
        res.end(myJSON);
    });
});

app.get('/contactsbyname/:name', function (req, res) {
    console.log("/contactsbyname/:name request received");

    var contacts = database.collection(CONTACT_COLLECTION_NAME);
    var re = new RegExp(req.params["name"], "i")
    contacts.find({ "Name": re }).toArray(function (err, items) {
        res.setHeader('Content-Type', 'application/json');
        var myJSON = JSON.stringify(items);
        res.end(myJSON);
    });
});

app.post('/contacts', function (req, res) {
    console.log("/contacts request received");

    if (req.body) {
        if (!req.body._id) {
            var contacts = database.collection(CONTACT_COLLECTION_NAME);
            contacts.insert(req.body, { w: 1 }, function (err, result) {
                if (!err) {
                    res.setHeader('Content-Type', 'application/json');
                    var myJSON = JSON.stringify(result.ops);
                    res.end(myJSON);
                }
                else
                    res.end(err.message);
            });
        }
        else
            res.end("Must insert a new contact and cannot specify id");
    }
    else
        res.end("Must insert a valid contact");

});

app.put('/contacts', function (req, res) {
    console.log("/contacts request received");

    if (req.body && req.body._id) {
        var contacts = database.collection(CONTACT_COLLECTION_NAME);
        var objectId = new ObjectId(req.body._id);
        var query = { "_id": objectId };
        var updatedContact = req.body;
        delete updatedContact['_id'];
        contacts.updateOne(query, updatedContact, function (err, result) {
            if (!err) {
                res.setHeader('Content-Type', 'application/json');
                var myJSON = JSON.stringify(result.result);
                res.end(myJSON);
            }
            else
                res.end(err.message)
        });
    }
    else
        res.end("Must send a valid contact and id");
});

var server = app.listen(8081, function () {
    var host = server.address().address;
    var port = server.address().port;

    // Connect to the db
    mongoClient.connect(MONGODB_LOCATION, function (err, db) {
        if (!err) {
            database = db;
            console.log("We are connected to the database");
        }
    });

    console.log("Started at http://%s:%s", host, port);
});