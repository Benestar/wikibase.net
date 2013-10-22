wikibase.net
============

wikibase.net is an api for the MediaWiki Wikibase extension based on 
.NET Framework 2.0. For more information about Wikibase see https://www.mediawiki.org/wiki/Wikibase. Wikibase is the software [Wikidata](https://www.wikidata.org) depends on.

This api is still in development and does not promise to be stable. Thus if you find a bug, please report it or even better, create a patch and make a pull request. Contributions are very welcome.

Overview
--------

The api has a more or less complex structure, so here is a short overview. Detailed information and documentation can be found in [the wiki](https://github.com/Benestar/wikibase.net/wiki).

The base class that is needed everywhere is the `WikibaseApi`. It provides internal access to the api and handles user data. With the `EntityProvider` you can get an instance of the `Entity` class, which is either an `Item` or a `Property`. Besides labels, descriptions and aliases for each language, Entities also consist of claims. A `Claim` has a main `Snak` that provides a `DataValue`. If the claim is a `Statement` (it always is a statement atm.), it can be proved by a `Reference` which itself contains several snaks. A claim actually also has some qualifiers but they are not supported yet.

Download and Installation
-------------------------

You can clone the source code from this repository to your local machine and compile it there. Note that the project depends on minimaljson.net, a JSON parser to get the api responses. You can download it [here](https://github.com/Benestar/minimaljson.net).

However, there is also a zipped compiled version of the api which is ready to use yet. Just download [the file](https://github.com/Benestar/wikibase.net/blob/master/Wikibase.NET.zip?raw=true) and unzip it. Then copy the DLLs into your project folder and add Wikibase.dll as a refer to your project. Finally you have to add the using directive at the top of your file: `using Wikibase;`.

License
-------

wikibase.net is licensed under the GNU GENERAL PUBLIC LICENSE Version 2 (GPL v2). A copy of the license can be found in the LICENSE file.
