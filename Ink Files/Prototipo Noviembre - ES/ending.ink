== game_ending ==
// Final reaction of group 1
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))

TODO IF THE PLAYER HAS NOT FORWARDED ANY NEWS, THE FAMILY SHOULD BE CHECKING ON THEM - AND OTHER GROUPS SHOULD NOTE THAT THE PLAYER HAS BEEN VERY QUIET.

Has leído {news_read} noticias de las {news_count} que has recibido. {news_read == news_count: Tu dedicación a la tarea es encomiable, recluta.} {news_read == 0: Creo que te falta comprometerte con la misión, recluta.} #parrafo

// The variable news_checked_received and news_fake_received below are not used anywhere yet, it's only been declared in the main file. For this to work, the database needs to count the number of checked news that the player gets, which I wasn't keeping track of. 

Has enviado {news_checked} noticias contrastadas. {news_checked_received == 0 : Parece que no te ha llegado ninguna noticia contrastada.} {news_checked_received == news_checked: Todos los artículos que has enviado están contrastados. ¡Excelente trabajo!}{news_checked_received < news_checked: No has enviado todas las noticias contrastadas que has recibido. Recuerda que también es beneficioso reenviar información fidedigna a tus allegados.} #parrafo 

Has enviado {news_fake} noticias falsas a otros. {news_fake == news_count: Si sigues enviando estas noticias, parece que estás saboteando nuestra misión para combatir la desinformación.}{news_fake == 0: Has interceptado todas fake news que te han llegado. Tu capacidad para interceptar la desinformación es extraordinaria. }#parrafo

Gracias por completar el entrenamiento F.A.B.U.S.
Esperamos que el proceso haya sido gratificante, y que ahora tengas mejor capacidad para poder identificar la desinformación y frenarla, a la vez que puedas aprovecharte de la información contrastada. 

{news_checked_received == news_checked && news_fake == 0 && news_read == news_count: Tu contribución a mantener a la sociedad informada es mangnífica. Has ganado la medalla de honor por tus labores contra la desinformación. Gracias a tu trabajo, podremos mantener a la población bien informada. El siguiente paso es que les enseñes a tus allegados lo que has aprendido en este entrenamiento. }

{news_checked_received == news_checked && news_fake == 0: Tu contribución a mantener a la sociedad informada es muy bunea.  Gracias a tu trabajo, podremos mantener a la población bien informada. El siguiente paso es que les enseñes a tus allegados lo que has aprendido en este entrenamiento. }

{news_fake == news_count: Parece que estás un poco lejos de poder convertirte en un defensor contra la desiformación. Te recomendamos que repitas el entrenamiento.}



-> END
