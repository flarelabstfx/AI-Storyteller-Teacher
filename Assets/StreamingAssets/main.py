import sys

def run_dolly():
    import torch
    from transformers import pipeline

    #if len(sys.argv) <= 2 or sys.argv[2] != "outputOnly":
    #    print("Start")

    #generate_text = pipeline(model="databricks/dolly-v2-12b", torch_dtype=torch.bfloat16, trust_remote_code=True, device_map="auto")
    generate_text = pipeline(model="databricks/dolly-v2-3b", torch_dtype=torch.bfloat16, trust_remote_code=True, device_map="auto")

    #if len(sys.argv) <= 2 or sys.argv[2] != "outputOnly":
    #    print("Step 1 Done")

    #res = generate_text("Summarise this for me: Algebra is the study of variables and the rules for manipulating these variables in formulas it is a unifying thread of almost all of mathematics. Elementary algebra deals with the manipulation of variables (commonly represented by Roman letters) as if they were numbers and is therefore essential in all applications of mathematics. Abstract algebra is the name given, mostly in education, to the study of algebraic structures such as groups, rings, and fields. Linear algebra, which deals with linear equations and linear mappings, is used for modern presentations of geometry, and has many practical applications (in weather forecasting, for example). There are many areas of mathematics that belong to algebra, some having 'algebra' in their name, such as commutative algebra, and some not, such as Galois theory. The word algebra is not only used for naming an area of mathematics and some subareas; it is also used for naming some sorts of algebraic structures, such as an algebra over a field, commonly called an algebra. Sometimes, the same phrase is used for a subarea and its main algebraic structures; for example, Boolean algebra and a Boolean algebra. A mathematician specialized in algebra is called an algebraist.")
    #res = generate_text("Say this five times: [10]")
    res = generate_text(sys.argv[2])

    #if len(sys.argv) <= 2 or sys.argv[2] != "outputOnly":
    #    print("Step 2 Done")

    print(res[0]["generated_text"], end="")
    

#@app.route("/", methods=("GET", "POST"))
def run_gbt():
    #import os, openai
    import openai
    #from flask import Flask, redirect, render_template, request, url_for

    # app = Flask(__name__)
    # openai.api_key = os.getenv("OPENAI_API_KEY")
    openai.api_key = "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"

    #print("prompt: " + sys.argv[1])
    #if request.method == "POST":
    #animal = request.form["animal"]
    response = openai.Completion.create(
        model="text-davinci-003",
        #prompt=generate_prompt("Give me the name of a cat."),
        #prompt=generate_prompt(sys.argv[2]),
        prompt=sys.argv[2],
        temperature=0.6,
    )
        #return redirect(url_for("index", result=response.choices[0].text))
    #print(response.choices[0].text + " : " + sys.argv[2])
    print(response.choices[0].text)

    #result = request.args.get("result")
    #return render_template("index.html", result=result)


def generate_prompt(prompt):
    return """{}""".format(
        prompt.capitalize()
    )


if __name__ == "__main__":
    match sys.argv[1]:
        case "Dolly":
            run_dolly()
        case "GBT":
            run_gbt()
