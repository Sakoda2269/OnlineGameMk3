from django.shortcuts import render

# Create your views here.
def gamepage(request):
    return render(request, "gamepage/index.html")