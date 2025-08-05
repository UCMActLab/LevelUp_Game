// The variables and lists below are there to set up the database. May have to change some of these. 

TODO Add date to the list of data that players need to check (if we consider it relevant) 


LIST articles = art1, art2, art3, art4, art5, art6, art7, art8, art9, art10, art11, art12, art13, art14, art15, art16, art17, art18, art19, art20, art21, art22, art23,  art24, art25,  art26,  art27//this list is the number of items in the database. New entry in the database means adding an item here. 




// The function below is to print out the source of the article, to facilitate both the labels in the database and localization. 


TODO THE FUNCTION BELOW IS JUST FOR THE PURPOSES OF THE PROTOTYPE, IN THE UNITY VERSION, THIS SHOULD PLUG IN TO THE EXTERNAL DATABASE

=== function article_data(print_article, what)  // this function is actually the database 
{print_article:
    - art1:
        ~ return data (what, "Asesinato de un niño en Mocejón (Toledo) por un inmigrante", "newspaper", "El Hotel Pattaya de Mocejón, Toledo, localidad donde ha sido asesinado a puñaladas un niño de 11 años, está albergando en estos momentos cerca de 30 inmigrantes ilegales según confirman fuentes cercanas al hotel!! Los vecinos de Mocejón hablan: 'El pasado lunes 5 de Agosto llegaron al pueblo 50 africanos en un autobús que los dejó en el Hotel Pattaya. Somos menos de 5.000 personas y vivíamos tranquilos. Ahora hay violaciones, robos y el asesinato de este niño de 10 años'", news, false)
    - art2:
        ~ return data (what, "La Seguridad Social anuncia que va a retirar la incapacidad permanente a más de 1 millón de personas", "newspaper", "El anuncio de la Seguridad Social que no te va a gustar. Van a retirar la incapacidad permanente a más de 1 millón de personas. La Seguridad Social acaba de anunciar que va a retirar a más de 1 millón de personas la pensión que cobran por la incapacidad permanente. Javier Frade - 02/12/2023", news, false)
    - art3:
        ~return data (what, "La clase media paga ya más del 50% de su sueldo a Hacienda tras las 69 subidas de impuestos de Sánchez", "social", "La clase media paga ya más del 50% de su sueldo a Hacienda tras las 69 subidas de impuestos de Sánchez España es el país de Europa que más sube la presión fiscal desde 2019. Un contribuyente medio abona casi 15.500 euros en impuestos al año, según apunta el Instituto Juan de Mariana.", news, false)
    - art4:
        ~return data (what, "Durante 2024 se pagará hasta 1.000 euros más de IRPF", "social", "Es un contenido que circula desde 2022 debido a una reforma del Gobierno. El primer texto que nos llegó fue este que hace referencia a que el cambio se debe por las elecciones generales de 2023: 'Algunos de ustedes habrán escuchado que para el año que viene el IRPF va a ser más bajo, pero quisiera leer darles de que esto es una trampa. Lo que se va a hacer es, en año electoral, reducir las retenciones de la empresa del IRPF. Es decir, usted va a tener más dinero en el bolsillo, pero porque va a pagar menos a través de la empresa.'", news, false)
    - art5:
        ~return data (what, "Arotz González Gorka, marido de Marlaska, es titular de una cuenta con ingresos anómalos", "social", "ENTIDAD: CITIBANAMEX DIRECCION: CONTRATO/TIPO: CONDICIONES: IDENTIFICACION: OP MONTERREY DEPOSITO A PLAZO RENOVACION AUTOMATICA 24 MESES 3567434 17/12/2018 INICIO: ESTADO: IMPORTE: TITULAR: VIGENTE 600.000$ AROTZ GONZALEZ GORKA (DESDE 17/01/2019) BANCO DOMICILIO: citibanamex VIS @Alvisepf&nbsp; Se llama Arotz González Gorka. Consta como titular de una cuenta bancaria en la entidad 'Citibanamex' de Monterrey con 40 ingresos anómalos de 600.000 euros", news, false)
     - art6:
        ~ return data (what, "Luchadora que vence a otras mujeres en este vídeo es trans", "social", "La ley trans puede desigualar mucho el deporte femenino. Me importa un bledo lo que diga @IreneMontero o cualquier alimaña de su ministerio @IgualdadGob, ninguna mujer de mi familia va a competir con personas trans, no es igualdad de condiciones y así me lo han comunicado", news, false)
    - art7:
        ~ return data (what, "HEADLINE 7", "newspaper", "Paragraph 7", science, true)
    - art8:
        ~return data (what, "El cambio climático no está causado por las actividades humanas, son ciclos naturales", "social", "Paragraph 8", science, true)
    - art9:
        ~return data (what, "Un coche eléctrico explota en Milán", "social", "Milán. Un coche eléctrico se incendia en la calle. Son un peligro y un fiasco. El fin de este circo, desde el golpe de la gobernanza mundial kovi 2020, es la transformación de hábitos y el incremento totalitario de control político, con los sociópatas amparados en 'portubien'.", science, true)
    - art10:
        ~return data (what, "HEADLINE 10", "social", "Paragraph 10", science, true)
    - art11:
        ~return data (what, "Casos de miocarditis y pericarditis con la vacuna contra la COVID-19 de Pfizer", "social", "La OMS estudia posibles problemas de corazón tras recibir la vacuna de Pfizer COVID - 19.", science, false)
    - art12:
        ~return data (what, "No, este correo electrónico que te avisa de 'irregularidades en su declaración de la renta' no es de la Agencia Tributaria: es phishing.", "social", "Paragraph 12", scam, true)
    - art13:
        ~return data (what, "HEADLINE 13", "social", "Paragraph 13", scam, true)
    - art14:
        ~return data (what, "HEADLINE 14", "social", "Paragraph 14", scam, true)
    - art15:
        ~return data (what, "Lidl está regalando cupones de 180 euros", "social", "Paragraph 15", scam, true)
    - art16:
        ~return data (what, "Banco Santander: Usuario deshabilitado por razones de seguridad", "social", "Paragraph 16", scam, false)
    - art17:
        ~return data (what, "Valencia ha sido producto de un ataque meteorológico HAARP", "social", "Paragraph 17", conspiracy, false)
    - art18:
        ~return data (what, "HEADLINE 18", "social", "Paragraph 18", conspiracy, true)
    - art19:
        ~return data (what, "Los alimentos con el símbolo de la rana de Rainforest Alliance tienen insectos molidos", "social", "Erdo hoy a las 13:35 9 de 10 El sello de la rana corresponde a la certificación Rainforest Alliance, este sello va en el etiquetado o enlatado de los alimentos. Si lo llevan significa que ese producto lleva toda clase de insectos Detrás de esto está Bill Gates. CERTIFIED Esto es una de las cosas que entran en la Agenda20-30. Aprobado por la UE. Las marcas más conocidas, de harinas, Chocolates, Cereales... Ya llevan insectos molidos. Comparte. X80 CROSSED MOR", conspiracy, true)
    - art20:
        ~return data (what, "Los pájaros no existen", "social", "Paragraph 20", conspiracy, false)
    - art21:
        ~return data (what, "El Foro Económico Mundial ordena quemar a las abejas para provocar una hambruna mundial", "social", "WEF ordena a los gobiernos quemar abejas para provocar una 'hambruna global' 🤔 ... a ver qué tienen que decir sobre esto los que son supostos protectores de la naturaleza ? EI FEM ordena al gobierno quemar millones de abejas para provocar una 'hambruna global' Los apicultores de todo el mundo informan que funcionarios gubernamentales visitan granjas destruyen colonias de abejas sanas sin motivo alguno y sin realizar pruebas para detectar las", conspiracy, false)
    - art22:
        ~return data (what, "Nos fumigan con chemtrails - varias fotos de aviones con esta narrativa", "social", "Los chemtrails nos envenenan. Una nube tóxica sobrevuela Sicilia y eso evita las lluvias. Esto es la verdadera causa del cambio climático, que en realidad es un invento para subirnos los impuestos.", conspiracy, false)
    - art23:
        ~return data (what, "La mujer de Pedro Sánchez, Begoña Gómez, es un travesti", "social", "La mujer del presidente es un travesti es un travesti y al más que tú lo puedes comprobar vete a Google pide fotografía de Begoña Gómez", conspiracy, false)
    - art24:
        ~return data (what, "Donald Trump posando con personas negras", "social", "Paragraph 24", computers, false)
    - art25:
        ~return data (what, "Grata zumbada pide aviones de ‘combate con pilas’ que lleven ‘misiles biodegradables", "social", "Paragraph 25", computers, false)
    - art26:
        ~return data (what, "Un adulto llevando a cuestas a cinco niños heridos en Palestina", "social", "Un adulto llevando a cuestas a cinco niños heridos en Palestina Imágenes que llegan de Gaza. Deberían llamarle héroe!!!", computers, false)
    - art27:
        ~return data (what, "Imagenes que llegan de Gaza", "social", "Paragraph 27", computers, false)
    - else: [Cannot find {print_article}]

    }
    


