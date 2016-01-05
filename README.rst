============
Crate.Client
============

.. image:: https://travis-ci.org/mfussenegger/crate-mono.svg?branch=master
    :target: https://travis-ci.org/mfussenegger/crate-mono
    :alt: travis-ci

.. image:: https://ci.appveyor.com/api/projects/status/y5i7o4clk4x84rwx/branch/master?svg=true
    :target: https://ci.appveyor.com/project/mfussenegger/crate-mono
    :alt: appveyor


Crate.Client is a Mono/.NET client driver implementing the ADO.NET interface
for `Crate <https://crate.io>`_

::

    using Crate.Client;

    using (var conn = new CrateConnection()) {
        conn.Open();
        using (var cmd = new CrateCommand("select name from sys.cluster", conn)) {
            var reader = cmd.ExecuteReader();
            reader.Read();
            string clusterName = reader.GetString(0);
        }
    }

Things missing
==============

Currently this is just a prototype. Things that are missing are:

* type infos in the Data Reader
* DataAdapter class
* a release and nuget package
* an EntityFramework Provider
