# remotethermo-client
This is a C# client for some of the remotethermo.com based Appliance (like Ariston NET) 

Thin integration is a side project which was tested for an heat pump boiler 
It logs in to appliance website (https://www.<your appliance brand>.remotethermo.com) and fetches/sets data on that site.
You are free to modify and distribute it. It is distributed 'as is' with no liability for possible damage.

## Simple Console app
The test application perform a connection to the remote thermo servers and retreive the PlantData, PlantSettings and the Schedule showing the current status of the heat pump and setpoints.
![image](https://user-images.githubusercontent.com/45007019/120938051-47ecba00-c711-11eb-9504-958db4a59f5b.png)

# Features
With the API i've exposed you can:
  - get the current status
  - switch on / off the boiler
  - change the plant mode (scheduled or manual)
  - update the Operating Mode (green, comfort, fast, iMemory)-
  - enable/disable the Antilegionella
  - enable/disable PreHeating 
  - download and upload the Schedule
 
## API slow nature
API connect to the website, which then connect via gateway to the boiler. The bus has problem handling high bandwidth and thus requests are sent after some specific periods of time. Periods were selected based on tests where not much of interfence was seen when using Ariston Net application or Google Home application or using https://www.ariston-net.remotethermo.com. Still interfences occaionally take place. It is normal to occasionally get connection errors due to devices chain involved.

## Tested with the following products
  - Ariston Velis
