import random

adj = ["Resoluto","Asim","Aciago","Lerdo","Pusil","Impoluto","Abstruso","Primoroso","Recalcitrante","Estridente","Grandilocuente","Locuaz","Sagas","Injurioso","Depravado","Oneroso","Mal","Jovial","Iluso","Zafio","Obstinado","Zozobra","Exorbitante","Flem","Ensortijado","Bizarro","Insulso","Abnegado","Indubitable","Deso","Ladino","sagaz","Indeleble","Palmaria"]
sus = ["Fleco","Abulia","Magnanimidad","Escepticismo","Mansedumbre","Acongojo","Ostentaci","Euforia","Tribulaci","Templanza","Prurito","Nitidez","Ramalazo","Afici","Probidad","Gula","Abyecci","Avidez","Abarrajar","Sufragio","Chozno","Vestigio","Macram","Arte","Prodigalidad","Prodigiosidad","Septenario","Predio","Tegumento","Regocijo","Interregno","Cacofon","Hegemon","Lacayo","Ilaci","Antropoide","Patetismo","Felon","Ponderaci","Albazo","Avisamiento","Abulafia","Brewda","Hermozo"]

types = 4
characteristics = 3

output = "SET IDENTITY_INSERT Characters ON\n"
indexes = [0] * types
id = 0
for c in range(types**characteristics):
    output += "INSERT INTO Characters (ID, name) VALUES(" + str(id) + ", '" + sus[int(random.random()*len(sus))] + " " + adj[int(random.random()*len(adj))] + "')\n"
    for i in range(len(indexes)):
        indexes[i] += 1
        if indexes[i] >= characteristics:
            indexes[i] = 0
            continue
        break
    i = 0
    for index in indexes:
        output += "INSERT INTO CharactersCharacteristics VALUES(" + str(id) + ", " + str(6+3*i+index) + ")\n"
        i += 1
    id += 1

f = open("generate_characters.sql", "w")
f.write(output)
