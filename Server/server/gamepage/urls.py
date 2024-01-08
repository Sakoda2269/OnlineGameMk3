from django.urls import path
 
from . import views
 
app_name = "gamepage"

urlpatterns = [
    path("", views.gamepage, name="index")
]