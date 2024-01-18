//laoding the data and displaying the topics list at the loading time.
$(document).ready(function () {
    $.ajax({
        url: "https://localhost:7237/api/RssFeed/getFeed",
        type: "GET",
        dataType: "json",
        success: function (data) {
            console.log("data =", data);

            // Assuming data is an object with a property 'data' containing the array of items
            var items = data;

            // Iterate over the array and create li elements
            for (var i = 0; i < items.length; i++) {
                var item = items[i];

                var $newsElement = $("<li class='news-item' data-id='" + item.id + "'>" + item.title + "</li>");
                $("#topics").append($newsElement);
            }

            // Attach the click event handler.
            $(".news-item").click(getPost);
        },
        error: function (error) {
            console.error("Error:", error);
        }
    });
});

//A function to handle the event of clicking on a post subject
function getPost() {

    var id = $(this).data("id");
    $.ajax({
        url: `https://localhost:7237/api/RssFeed/getPost/${id}`,
        type: "GET",
        dataType: "json",
        success: function (data) {
            //displaying the post details
            $("#body").html(data.description);
            $("#title").text(data.title);
            $("#link").text("link to original post");
            $("#link").attr("href", data.link);
        },
        error: function (error) {
            console.error("Error:", error);
        }
    });
}
