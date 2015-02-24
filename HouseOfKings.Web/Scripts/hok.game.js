$(function () {
    var game = $.connection.game; // the generated client-side hub proxy
    $groupCircle = $('.group-circle'),
    groupName = $('#group-name').val(),
    $btn = $('.action-pick-card');

    $btn.button('waiting').prop('disabled', true);

    function init() {
        game.server.joinGroup(groupName).done(function () {
            setAudit('Joined ' + groupName);

            $(document)
                .on('click', '.action-pick-card', function () {
                    $btn.button('waiting').prop('disabled', true);

                    game.server.pickCard(groupName);
                })
                .on('click', '#action-replay', function () {
                    shuffleDeck();
                });
        });
    }

    function shuffleDeck() {
        setAudit('Shuffled deck');
        setMessage('');
        updateCard(null);
        updateStats({ kingCount: 4, cardCount: 52 });
    }

    function drawGroup(groupInfo) {
        groupInfo.players.forEach(function (player) {
            addPlayer(player);
        });
        drawPlayers();
        drawTurn(groupInfo.turn);
    }

    function drawPlayers() {
        var players = $('.player'),
        width = $groupCircle.width(),
        height = $groupCircle.width(),
        radius = (width / 2) * 0.7,
        angle = 0,
        step = (2 * Math.PI) / players.length;
        players.each(function () {
            var x = Math.round(width / 2 + radius * Math.cos(angle) - $(this).width() / 2);
            var y = Math.round(height / 2 + radius * Math.sin(angle) - $(this).height() / 2);

            $(this).css({
                left: x + 'px',
                top: y + 'px'
            });
            angle += step;
        });
    }

    function parseSuit(suit) {
        $('.suit').removeClass('suit-heart suit-diamond suit-spade suit-club');

        switch (suit) {
            case 0: {
                $('.suit').addClass('suit-club');
                break;
            }
            case 1: {
                $('.suit').addClass('suit-diamond');
                break;
            }
            case 2: {
                $('.suit').addClass('suit-heart');
                break;
            }
            case 3:
            default: {
                $('.suit').addClass('suit-spade');
                break;
            }
        }
    }

    function parseNumber(number) {
        switch (number) {
            case 1: {
                number = 'A';
                break;
            }
            case 11: {
                number = 'J';
                break;
            } case 12: {
                number = 'Q';
                break;
            }
            case 13: {
                number = 'K';
                break;
            }
            default: {
                break;
            }
        }

        return number;
    }

    function updateStats(turn) {
        $('#card-count').html(turn.cardCount);
        $('#king-count').html(turn.kingCount);
    }

    function updateCard(card) {
        if (card) {
            var number = null;
            parseSuit(card.suit);
            number = parseNumber(card.number);
            $('.suit').html(number);
        }
    }

    function addPlayer(player) {
        game.client.setAudit(player.name + ' joined the game');
        $groupCircle.append('<div class="player" data-player-id="' + player.id + '" data-name="' + player.name + '"></div>');
    }

    game.client.drawGroup = function (groupInfo) {
        drawGroup(groupInfo);
    }

    game.client.addPlayer = function (player) {
        addPlayer(player)
        drawPlayers();
    }

    game.client.removePlayer = function (player) {
        setAudit(player.name + ' left the group');
        $groupCircle.find("[data-player-id='" + player.id + "']").remove();
    }

    game.client.setTurn = function () {
        $btn.button('pick').prop('disabled', false);
    }

    function setGameover(player) {
        $('#gameover-message').html(player.name + ' picked the last King<br/><br/><strong>Game Over</strong>');
        $('#gameover-modal').modal({ keyboard: false, backdrop: 'static' });
    }

    game.client.setGameover = function (player) {
        setGameover(player);
    }

    function drawTurn(turn) {
        updateStats(turn);

        var card = turn.card;

        if (card) {
            setAudit(turn.player.name + ' picked ' + '<span class="suit"></span>');
            setMessage('<p class="text-center">' + turn.player.name + ' picked&emsp;' + '<span style="display:inline;" class="suit"></span><br/><em class="small">card rule&nbsp;</em><strong>&emsp;' + turn.rule.title + '</strong></p>');

            updateCard(card);
        }

        if (turn.fin) {
            setGameover(turn.player);
        }
    }

    game.client.drawTurn = function (turn) {
        drawTurn(turn);
    }

    function setAudit(message) {
        $('#audit').html(message);
    }

    game.client.setAudit = function (message) {
        setAudit(message);
    }

    function setMessage(message) {
        $('#message').html(message);
    }

    game.client.setMessage = function (message) {
        setMessage(message);
    }

    function setNextTurn(player) {
        setAudit('Waiting for ' + player.name + ' to pick a Card');
    }

    game.client.setNextTurn = function (player) {
        setNextTurn(player);
    }

    $.connection.hub.connectionSlow(function () {
        setAudit('Poverty connection to server...');
    });

    $.connection.hub.reconnected(function () {
        setAudit('Disconnected from server')
    });

    $.connection.hub.disconnected(function () {
        setAudit('Attempting to reconnect to server...');
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000);
    });

    // Start the connection
    $.connection.hub.start().done(init);
});