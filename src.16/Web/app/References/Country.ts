module Luxena.Views
{

    registerEntityControllers(sd.Country, se => ({

        list: [
            se.TwoCharCode,
            se.ThreeCharCode,
            se.Name,
        ],

        view: () => sd.tabPanel().card().items(

            sd.col().icon(se).items(
                se.Name.header2(),
                sd.hr(),
                se.TwoCharCode,
                se.ThreeCharCode
            ),
            
            se.Airports.field()
                .items((se: IAirportSemantic) => [
                    se.Code, se.Name, se.Settlement,
                ])
                //.gridController({ height: 500 })
        ),

        edit: [
            se.Name,
            se.TwoCharCode,
            se.ThreeCharCode,
        ],

    }));

}