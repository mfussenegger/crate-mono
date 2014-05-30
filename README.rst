==========
Crate-mono
==========

Crate-mono is a Mono/.NET client driver implementing the ADO.NET interface for
`crate data <https://crate.io>`_

::

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
* proper testing (currently it requires a local crate instance running on port
  4200 to run the rests)
* an EntityFramework Provider
