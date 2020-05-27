# OpenStreetMap_Toolkit
## Getting started
### See the example files in the examples folder of this toolkit:
- Search in a bounding box
![bounding box search](https://github.com/BHoM/OpenStreetMap_Toolkit/blob/Update_templates_and_readme/examples/boundingbox.jpg)
- Search by centre radius
![centre radius search](https://github.com/BHoM/OpenStreetMap_Toolkit/blob/Update_templates_and_readme/examples/radius.jpg)
- Search in a polygon
![polygon search](https://github.com/BHoM/OpenStreetMap_Toolkit/blob/Update_templates_and_readme/examples/polygon.jpg)
- Search along a line string
![line string search](https://github.com/BHoM/OpenStreetMap_Toolkit/blob/Update_templates_and_readme/examples/linestring.jpg)
### Download a building massing example
- [Building massing](https://burohappold.sharepoint.com/:f:/r/sites/Direction/designtechnology/Script%20Library/00348_OpenStreetMap%20Scripts?csf=1&web=1&e=SLhE1T)
![building massing](https://github.com/BHoM/OpenStreetMap_Toolkit/blob/Update_templates_and_readme/examples/osm.gif)
## Resources + background
- https://www.openstreetmap.org/
- [Features mapped in OpenStreetMap](https://wiki.openstreetmap.org/wiki/Map_Features)
- [OpenStreetMap Elements used by this toolkit](https://wiki.openstreetmap.org/wiki/Elements)
- [Test OpenStreetMap queries online](https://overpass-turbo.eu/)
  - Remove `https://www.overpass-api.de/api/interpreter?data=[out:json];` from the query string
  - Paste the remaining part in https://overpass-turbo.eu/. 
  - For example copy:
`(way[building](around:150,53.3352318,-6.228456);rel[building](around:150,53.3352318,-6.228456););(._;>;);out body;`
- [Geographic_coordinate_system](https://en.wikipedia.org/wiki/Geographic_coordinate_system)
