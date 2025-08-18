=== checkin_2 ===
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))

{article_forwarded_group1 == 0: {group1_speaker}: You're a bit quiet.}
{article_forwarded_group2 == 0: {group2_speaker}: Are you okay?}
{article_forwarded_group3 == 0: {group3_speaker}: Haven't seen you in a bit. }


El grupo de amigos <>{
    - group_1 >= 1:
     está a gusto contigo.
    - group_1 == 0:
     son indiferentes a lo que digas.  
    - else:
    recelan de lo que les mandas.  
}
Tu familia <>{
    - group_2 >= 1:
     está tranquila por ti.
    - group_2 == 0:
     ni se preocupan ni se despreocupan por ti.
    - else:
     está preocupada por ti. 
}

Tus vecinos <>{
    - group_3 >= 1:
     están contentos de tenerte en el grupo. 
    - group_3 == 0:
     no tiene mucha opinión de tu participación en el grupo. 
    - else:
     están un poco incómodos contigo.  
}


{s3_g3: Omar: Te quiero dar las gracias por enviar el artículo sobre inmigración. Le ha sido de ayuda a mi amiga para conseguir cita y así poder conseguir su permiso de trabajo.}

TODO Fix the conditional below, so the story reflects whether the mask articles have been sent or not. 
{ s4_g1: María: Marcos está en el hospital. Se tomó la medicina del artículo que enviaste para ponerse bien más rápido, pero se pasó con la dosis y le han salido unas ronchas tremendas. Los médicos le van a tener en observación esta noche.}


-> scene_5_intro
