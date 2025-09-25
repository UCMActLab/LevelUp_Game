// The variables and lists below are there to set up the database. May have to change some of these. 

TODO Add date to the list of data that players need to check 


LIST articles_HD = HC_art1, HC_art2, HC_art3, HC_art4, HC_art5, HC_art6, HC_art7, HC_art8, HC_art9, HC_art10 //this list is the number of items in the database. New entry in the database means adding an item here. 


TODO THE FUNCTION BELOW IS JUST FOR THE PURPOSES OF THE PROTOTYPE, IN THE UNITY VERSION, THIS SHOULD PLUG IN TO THE EXTERNAL DATABASE

=== function article_data_HC(print_article, what)  // this function is actually the database 
{print_article:
    - HC_art1:
        ~ return data (what, "¿SUFRISTE PÉRDIDAS DURANTE EL APAGÓN? PUEDES RECIBIR COMPENSACIÓN", "social", "Puede que hayas sido uno de los que fueron afectados por la pérdida de electricidad todo un día. Si sufriste pérdidas de algún tipo (como comida en el congelador que se c, tuviste que dormir en una estación de tren, o perdiste clientes) y naciste antes de 1975, puedes solicitar fondos de compensación al Ministerio. Puedes hacer la solicitud a través de esta página blackoutfunds.net y te enviarán un enlace personalizado a través del que puedes enviar tu informaci´n y explicar cómo te afectó el apagón. Puedes recibir hasta 2000 euros, independientemente de tus ingresos, en dos semanas. El apagón fue otra consecuencia de las pobres políticas de energía Europeas. La guerra en Ucrania ha comprometido nuestras fuentes de energía en toda la unión, y sus miembros comparten energia entre países de maneras que son inadecuadas e insostenibles. Los ciudadanos no deberían pagar por guerras fuera de la Unión Europea y por las malas decisiones de los políticos.", scam, false)
    - HC_art2:
        ~ return data (what, "SE RECOMIENDA EL USO DE MASCARILLAS POR LA NUEVA VARIANTE DE COVID", "newspaper", "La Organización Mundial de la Salud (OMS) ha publicado una recomendación a la población para que utilicen mascarillas con nivel de filtro FFP2 para aquellos de más de 60 años de edad y con inmunodeficiencias, después del incremento en casos de una nueva variedad, NB 1.8.1., que se originó en China en Marzo. Esto es una medida de precaución para proteger a los más vulnerables, así como para ralentizar la propagación del virus. Esta variedad de COVID-19 se ha propagado de manera acelerada, y ha llegado a los EEUU a través de sus aeropuertos internacionales. La variedad NB.1.8.1 se asocia con síntomas similares a aquellos vistos en previas variantes, de acuerdo con el Dr. Young-soo Shin, director de la OMS en la región del Pacífico. Los síntomas más comunes incluyen problemas respiratorios, como tos y dolor de garganta, así como efectos en el sistema corporal como fiebre y fatiga.", science, true)
    - HC_art3:
        ~return data (what, "EL GOBIERNO ANUNCIA NUEVAS MEDIDAS PARA LEGALIZAR A INMIGRANTES INDOCUMENTADOS", "newspaper", "El gobierno ha anunciado nuevas medidas para facilitar el registro y la legalización de inmigrantes que ya están en el país. La nueva legislación permitirá registrarse a los inmigrantes que entraron en el país de manera ilegal y así obetner permisos de trabajo. El objetivo de estas medidas es el frenar el empleo ilegal, y prevenir situaciones de abuso donde los empleados ilegalmente son obligados a trabajar bajo amenazas de ser deportados. Los imigrantes que acaben de legalizarse podrán tener aceso a la Seguridad Social, y contribuir con los impuestos. Estos inmigrantes pueden solicitar regularizar su situación a partir de hoy al Ministerio del interior, tanto online como por correo. Las oficinas de la Seguridad Social también contarán con un mostrador para ayudar a inimigrantes con sus documentos, y así acelerar el proceso de legalización.", news, true)
    - HC_art4:
        ~return data (what, "IVERMECTIN, LA MEDICINA MILAGROSA", "social", "Si no tienes una botella de Ivermectin en tu alacena, ya estás perdiendo tiempo. El Ivermectin te puede proteger de futuras enfermedades, incluyendo cepas del COVID, y adquirilo es tan fácil como ir a la tienda de animales de tu barrio. Se ha utilizado durante décadas para tratar infectiones con parásitos, como la sarna y la ceguera de los ríos. La medicina paraliza y mata a los parásitos y se ha comprobado que es muy efectiva y segura cuando se utiliza según las indicaciones. Es barata, y se dice que puede ser efectiva contra infecciones y enfermedades en seres humanos. ", science, false)
    - HC_art5:
        ~return data (what, "EL CAMBIO CLIMÁTICO ES MENTIRA: CALZADA ROMANA ENCONTRADA BAJO GLACIAR", "blog", "Los restos de una calzada romana han sido encontrados a 3000 metros bajo el nivel del mar en Suiza, después de que se derritiera uno de los glaciares. Esto depuestra que no había hilo allí en el siglo I, y que los Alpes son el resultado de la Gran Glaciación Cuaternaria hasta la Edad de Hielo Menor (1950). Esto es otra prueba más en contra de los alarmistas del cambio climático, explica un experto, que también afirma que en la época de los Romanos no se construyó un túnel para atravesar el glaciar, simplemente porque no había hielo. Es imposible que hayamos pasado por el julio más caluroso en 120.000 años, mientras que hace 2000 años el glaciar que cubría esta calzada no existiera.", conspiracy, false)
    - HC_art6:
        ~ return data (what, "HEADLINE 1", "blog", "Paragraph 1", news, true)
    - HC_art7:
        ~ return data (what, "HEADLINE 2", "newspaper", "Paragraph 2", science, true)
    - HC_art8:
        ~return data (what, "HEADLINE 3", "social", "Paragraph 3", conspiracy, true)
    - HC_art9:
        ~return data (what, "HEADLINE 4", "social", "Paragraph 4", computers, true)
    - HC_art10:
        ~return data (what, "HEADLINE 5", "social", "Paragraph 5", scam, true)
    
    - else: [No se encuentra {print_article}]

    }
    


