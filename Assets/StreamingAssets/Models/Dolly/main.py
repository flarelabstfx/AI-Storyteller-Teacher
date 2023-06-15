import torch
from transformers import pipeline
import sys

if len(sys.argv) == 1 or sys.argv[1] != "outputOnly":
    print("Start")

#generate_text = pipeline(model="databricks/dolly-v2-12b", torch_dtype=torch.bfloat16, trust_remote_code=True, device_map="auto")
generate_text = pipeline(model="databricks/dolly-v2-3b", torch_dtype=torch.bfloat16, trust_remote_code=True, device_map="auto")

if len(sys.argv) == 1 or sys.argv[1] != "outputOnly":
    print("Step 1 Done")

#res = generate_text("Summarise this for me: Algebra is the study of variables and the rules for manipulating these variables in formulas it is a unifying thread of almost all of mathematics. Elementary algebra deals with the manipulation of variables (commonly represented by Roman letters) as if they were numbers and is therefore essential in all applications of mathematics. Abstract algebra is the name given, mostly in education, to the study of algebraic structures such as groups, rings, and fields. Linear algebra, which deals with linear equations and linear mappings, is used for modern presentations of geometry, and has many practical applications (in weather forecasting, for example). There are many areas of mathematics that belong to algebra, some having 'algebra' in their name, such as commutative algebra, and some not, such as Galois theory. The word algebra is not only used for naming an area of mathematics and some subareas; it is also used for naming some sorts of algebraic structures, such as an algebra over a field, commonly called an algebra. Sometimes, the same phrase is used for a subarea and its main algebraic structures; for example, Boolean algebra and a Boolean algebra. A mathematician specialized in algebra is called an algebraist.")
res = generate_text("Say this five times: [10]")

if len(sys.argv) == 1 or sys.argv[1] != "outputOnly":
    print("Step 2 Done")

print(res[0]["generated_text"], end="")