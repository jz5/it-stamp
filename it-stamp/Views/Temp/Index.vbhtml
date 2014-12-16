@ModelType SearchEventsViewModel
@code 
    
End Code
<div class="row">
    <div class="col-sm-12 col-md-8">
        <ul>
            @For Each e As [Event] In ViewBag.Events

                @<li>
                    @e.Name
                </li>
            Next
        </ul>
    </div>
    <div class="col-sm-12 col-md-4">
        
    </div>
</div>
